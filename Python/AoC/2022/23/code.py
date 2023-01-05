# Advent of code Year 2022 Day 23 solution
# Author = Trent Huhn
# Date = December 2022

# This is not very efficient, especially part 2, but it does eventually get the right answer

import time

def print_elves(elves):
    print()
    (y_max, y_min),  (x_max, x_min) = [(max(x), min(x)) for x in zip(*elves.values())]
    for j in range(y_min, y_max+1):
        print(f"{j:02}",end='')
        for i in range(x_min, x_max+1):
            if (j,i) not in elves.values(): print('.',end='')
            else: print('#',end='')
        print()

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

NUM_TURNS = 10e6 # Set to 10 for part 1, make arbitrarily high for part 2

# Set up initial state
elves = {}
k = 0
for i in range(len(input)):
    line = input[i]
    for j in range(len(line)):
        if line[j] == '#': 
            elves[k] = (i,j)
            k+=1

done = False
neighbors = [(-1,-1),(-1,0),(-1,1),(0,-1),(0,1),(1,-1),(1,0),(1,1)]
check_dirs = [[(-1,-1),(-1,0),(-1,1)],[(1,-1),(1,0),(1,1)],[(-1,-1),(0,-1),(1,-1)],[(-1,1),(0,1),(1,1)]]
cur_dir = 0
turns = 1
finished = set()
while not done:
    done = True
    proposed = {}
    for idx,(elf,pos) in enumerate(elves.items()):     
        #if elf in finished: continue

        # Part 0: check if elf is in its final position
        has_neighbor = False
        for i in range(len(neighbors)):
            if (pos[0]+neighbors[i][0],pos[1]+neighbors[i][1]) in elves.values():
                has_neighbor = True
                done = False
                proposed[elf]=pos
                break
        
        if not has_neighbor: finished.add(elf)
        
    # if all elves are in final place, then we can break
    if done or turns == NUM_TURNS:
        break    

    # Part 1: propose moves
    for idx,(elf,pos) in enumerate(proposed.items()):
        
        dir = cur_dir
        for j in range(len(check_dirs)): # Try each of 4 directions (rotating)
            can_move = True
            for k in range(len(check_dirs[dir])):
                if (pos[0]+check_dirs[dir][k][0],pos[1]+check_dirs[dir][k][1]) in elves.values(): # occupied
                    can_move = False
                    break

             # assign new position for this elf if they move in this direction
            if can_move: 
                proposed[elf] = (pos[0] + check_dirs[dir][1][0], pos[1]+check_dirs[dir][1][1])
                break
            dir = (dir + 1) % 4
        if not can_move: # we couldn't move in any direction
            proposed[elf] = None # remove from our dict of proposed moves

    cur_dir = (cur_dir + 1) % 4 # increment starting direction to check for next turn

    # Part 2: attempt moves
    for idx,(elf,pos) in enumerate(proposed.items()):
        if pos != None and sum(1 for v in proposed.values() if v == pos) == 1: # this is the only elf proposing to move to this position
            elves[elf] = pos # update master list of elves

    turns += 1
    if turns % 20 == 0: print("Turn {t}, number of proposed moves: {m}".format(t = turns, m = len(proposed)))
    if turns % 100 == 0: print_elves(elves)

print_elves(elves)

# Count empty spaces
empty_spaces=0
(y_max, y_min),  (x_max, x_min) = [(max(x), min(x)) for x in zip(*elves.values())]
for j in range(y_min, y_max+1):
    for i in range(x_min, x_max+1):
        if (j,i) not in elves.values(): empty_spaces += 1        


part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(empty_spaces)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()



part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(turns)))