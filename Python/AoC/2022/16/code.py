# Advent of code Year 2022 Day 16 solution
# Author = Trent Huhn
# Date = December 2022

import time

# cur_state is a tuple containing: valves that have already been opened, current number of minutes, current pos
def dfs(graph, rate, cache, cur_state):
    minutes = cur_state[1]
    if minutes == 0: return 0 # we're out of time

    if cur_state in cache.keys(): # check if our current state is already in our cache
        return cache[cur_state] # return current "total release rate" corresponding to this state
    
    cur_valve = cur_state[2]
    opened = cur_state[0]
    cur_pressure = 0

    # check if we can open the current valve and whether we should (flow rate >0)
    if (cur_valve not in opened) and (graph[cur_valve][0] > 0):
        new_rate = rate + graph[cur_valve][0]
        opened = opened + (cur_valve,) # add value to our (immutable) tuple contained opened valves
        opened = tuple(sorted(opened)) # always make sure our tuple of opened valves is sorted
        new_state = (opened, minutes - 1, cur_valve)
        cur_pressure = dfs(graph, new_rate, cache, new_state)
    else: # if we didn't open a valve, then we should move to the next valve
        for i,next_valve in enumerate(graph[cur_valve][1]):
            new_state = (opened, minutes-1, next_valve)
            rel = dfs(graph, rate, cache, new_state)
            if rel > cur_pressure: cur_pressure = rel

    cur_pressure += rate
    cache[cur_state] = cur_pressure # add current state to the cache

    return cur_pressure    

# cur_state is a tuple containing: valves that have already been opened, current number of minutes, my pos, elephant pos
# NOTE: This takes a lot of memory and time (on i5-1235U processor w/ 8 GB RAM, takes around 11 minutes and ~4 GB of memory)
def dfs2(graph, rate, cache, cur_state):
    minutes = cur_state[1]
    if minutes == 0: return 0 # we're out of time

    if cur_state in cache.keys(): # check if our current state is already in our cache
        return cache[cur_state] # return current "total release rate" corresponding to this state
    
    my_pos = cur_state[2]
    ele_pos = cur_state[3]
    opened = cur_state[0]
    cur_pressure = 0

    # if we're at the same valve, always have the elephant move ahead while I open it (if necessary)
    me_open = my_pos not in opened and graph[my_pos][0] > 0
    ele_open = ele_pos not in opened and graph[ele_pos][0] > 0 and my_pos != ele_pos
  
    # we both open (different) valves
    if(me_open and ele_open):        
        new_rate = rate + graph[my_pos][0] + graph[ele_pos][0] # calculate new total flow rate
        opened = opened + (my_pos,ele_pos,) # add valves to our (immutable) tuple contained opened valves
        opened = tuple(sorted(opened)) # always make sure our tuple of opened valves is sorted
        new_state = (opened, minutes - 1, my_pos, ele_pos)
        cur_pressure = dfs2(graph, new_rate, cache, new_state)
    # I'll open a valve, elephant will move
    elif(me_open and not ele_open):        
        new_rate = rate + graph[my_pos][0]
        opened = opened + (my_pos,) # add value to our (immutable) tuple contained opened valves
        opened = tuple(sorted(opened)) # always make sure our tuple of opened valves is sorted
        for i,next_ele_pos in enumerate(graph[ele_pos][1]):
            new_state = (opened, minutes-1, my_pos, next_ele_pos)
            rel = dfs2(graph, new_rate, cache, new_state)
            if rel > cur_pressure: cur_pressure = rel
    # Elephant opens it's valve, I move
    elif(not me_open and ele_open):
        new_rate = rate + graph[ele_pos][0]
        opened = opened + (ele_pos,) # add value to our (immutable) tuple contained opened valves
        opened = tuple(sorted(opened)) # always make sure our tuple of opened valves is sorted
        for i,next_my_pos in enumerate(graph[my_pos][1]):
            new_state = (opened, minutes-1, next_my_pos, ele_pos)
            rel = dfs2(graph, new_rate, cache, new_state)
            if rel > cur_pressure: cur_pressure = rel
    else: # both of us move
        for i,next_my_pos in enumerate(graph[my_pos][1]):
            for j,next_ele_pos in enumerate(graph[ele_pos][1]):
                new_state = (opened, minutes-1, next_my_pos, next_ele_pos)
                rel = dfs2(graph, rate, cache, new_state)
                if rel > cur_pressure: cur_pressure = rel

    cur_pressure += rate
    cache[cur_state] = cur_pressure # add current state to the cache

    return cur_pressure    


with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

valves = {}
graph = {}
for i in range(len(input)):
    line = input[i].split(' ')
    valve_name = line[1]
    flow_rate = int(''.join(char for char in line[4].split('=')[1] if char.isdigit()))
    linked_valves = [''.join(char for char in line[i] if char != ',') for i in range(9,len(line))]
    valves[valve_name] = (flow_rate, linked_valves)

# initial cur_state object set to opened valves (empty tuple), # of minutes remaining (30), and current pos ('AA')
# initial flow rate set to 0
max_pressure = dfs(valves, 0, {}, (tuple(),30,'AA'))

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(max_pressure)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

max_pressure = 0
# for part two, our cache key has an extra item in order to store both mine and the elephant's position
max_pressure = dfs2(valves, 0, {}, (tuple(),26,'AA','AA'))

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(max_pressure)))