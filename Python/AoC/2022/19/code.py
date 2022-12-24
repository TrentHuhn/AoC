# Advent of code Year 2022 Day 19 solution
# Author = Trent Huhn
# Date = December 2022

import time

# blueprint is tuple containings: cost of ore bot, cost of clay bot, cost of obsidian bot (ore, clay), cost of geode bot (ore, obsidian)
# cur_state is a tuple containing: amount of ore, amount of clay, amount of obsidian,
# # of ore-producing robots, # of clay-producing robots, # of obsidian robots, # of geode robots, # of minutes remaining
def dfs(blueprint, cache, cur_state):
    minutes = cur_state[6]
    if minutes == 1: return 0 # we're out of time

    if cur_state in cache.keys(): # check if our current state is already in our cache        
       return cache[cur_state] # return current "total geodes cracked" corresponding to this state
    
    ore = cur_state[0]
    clay = cur_state[1]
    obsidian = cur_state[2]
    ore_bots = cur_state[3]
    clay_bots = cur_state[4]
    obsidian_bots = cur_state[5]

    ore_bot_cost = blueprint[0]
    clay_bot_cost = blueprint[1]
    obsidian_bot_cost = blueprint[2] # tuple (ore, clay)
    geode_bot_cost = blueprint[3] # tuple (ore, obsidian)

    cur_geodes = []

    # Calculate the most ore we could possibily spend (assuming we build the most ore-expensive bot every remaining minute,
    # minus the amount of ore we will collect in future minutes)
    # We can safely discard any more ore than this to help reduce our state space.
    # Repeat for other resources.
    max_ore_cost = max(geode_bot_cost[0], obsidian_bot_cost[0], clay_bot_cost, ore_bot_cost)
    max_possible_ore = max_ore_cost*minutes - ore_bots*(minutes - 1)
    max_possible_clay = obsidian_bot_cost[1]*minutes - clay_bots*(minutes - 1)
    max_possible_obsidian = geode_bot_cost[1]*minutes - obsidian_bots*(minutes - 1)
    ore = ore if ore < max_possible_ore else max_possible_ore
    clay = clay if clay < max_possible_clay else max_possible_clay
    obsidian = obsidian if obsidian < max_possible_obsidian else max_possible_obsidian

    # When pruning possible paths, do not produce more robots for a resource than any robot has costs in this resource 
    # (as otherwise, we will produce more resources than we ever need)

    # try building geode bot
    if ore >= geode_bot_cost[0] and obsidian >= geode_bot_cost[1]:
        # harvest resources, add new geode bot, decrement time
        new_state = (ore + ore_bots - geode_bot_cost[0], clay + clay_bots, obsidian + obsidian_bots - geode_bot_cost[1], \
             ore_bots, clay_bots, obsidian_bots, minutes - 1)  
        pos_geodes = dfs(blueprint, cache, new_state) # returns possible geodes via this path
        pos_geodes += (minutes - 1)
        return pos_geodes
        
    # try building obsidian bot (no point in building one in the last minute)
    if ore >= obsidian_bot_cost[0] and clay >= obsidian_bot_cost[1] and minutes > 1 and obsidian_bots < geode_bot_cost[1]:      
        new_state = (ore + ore_bots - obsidian_bot_cost[0], clay + clay_bots - obsidian_bot_cost[1], obsidian + obsidian_bots, \
             ore_bots, clay_bots, obsidian_bots + 1, minutes - 1) 
        cur_geodes.append(dfs(blueprint, cache, new_state))

    # try building clay bot (no point in building one in the last 2 minutes)
    if ore >= clay_bot_cost and minutes > 2 and clay_bots < obsidian_bot_cost[1]:
        new_state = (ore + ore_bots - clay_bot_cost, clay + clay_bots, obsidian + obsidian_bots, \
             ore_bots, clay_bots + 1, obsidian_bots, minutes - 1) 
        cur_geodes.append(dfs(blueprint, cache, new_state))

    # try building ore bot (no point in building one in the last minute)
    if ore >= ore_bot_cost and minutes > 1 and ore_bots < max_ore_cost:
        new_state = (ore + ore_bots - ore_bot_cost, clay + clay_bots, obsidian + obsidian_bots, \
             ore_bots + 1, clay_bots, obsidian_bots, minutes - 1) 
        cur_geodes.append(dfs(blueprint, cache, new_state))

    # do nothing
    new_state = (ore + ore_bots, clay + clay_bots, obsidian + obsidian_bots, \
            ore_bots, clay_bots, obsidian_bots, minutes - 1) 
    cur_geodes.append(dfs(blueprint, cache, new_state))
    
    geodes = max(cur_geodes)
    cache[cur_state] = geodes # add current state to the cache
    return geodes

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

blueprints = []
for i in range(len(input)):
    line = ''.join(char for char in input[i] if char.isdigit() or char in ['-',' ']) # keep just digits and spaces
    line = list(map(int,' '.join(line.split()).split(' '))) # remove duplicate spaces and then split into list
    blueprints.append((line[1], line[2], (line[3], line[4]), (line[5], line[6])))

# initial cur_state object set to amount of resources (none), # of bots (1 ore-producing), and current minute (24)
# initial # of geodes set to 0
max_geodes = []
for i in range(len(blueprints)): # loop through each possible blueprint
    best = dfs(blueprints[i], {}, (0, 0, 0, 1, 0, 0, 24))
    max_geodes.append(best)
    print("Blueprint {num}: {best}".format(num = i+1, best=best))

# sum up quality scores of each blueprint
quality_sum = sum([(i+1)*val for (i,val) in enumerate(max_geodes)])

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(quality_sum)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

total = 1
assert len(blueprints) >= 3
for i in range(3): # loop through just the first 3 blueprints 
    best = dfs(blueprints[i], {}, (0, 0, 0, 1, 0, 0, 32))
    total = total*best
    print("Blueprint {num}: {best}".format(num = i+1, best=best))

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(total)))