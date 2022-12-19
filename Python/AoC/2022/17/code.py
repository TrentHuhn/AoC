# Advent of code Year 2022 Day 17 solution
# Author = Trent Huhn
# Date = December 2022

import time

def build_tower(NUM_ROCKS,fingerprint = ([],-1,-1)):    
    # We only need to keep track of the top X rows of the tower, where X is distance from the last fully filled in row to the top of the tower
    # To start, define the floor at level 0
    tower = {0: [True]*7} 
    if fingerprint[0] != []:
        for j in range(len(fingerprint[0])):
            tower[len(fingerprint[0]) - j] = fingerprint[0][j]
    jet = 0 if fingerprint[1] == -1 else fingerprint[1]
    starting_rock = 0 if fingerprint[2] == -1 else fingerprint[2]
    for i in range(starting_rock, NUM_ROCKS + starting_rock):
        rock = [[None]*7]*4 # rock is at most 4 rows high

        tower_height = max(tower)
        #    ####
        if(i % 5 == 0):
            rock[3] = [None,None,tower_height+4,tower_height+4,tower_height+4,tower_height+4,None]
        #    .#.
        #    ###
        #    .#.
        elif(i % 5 == 1):
            rock[1] = [None,None,None,tower_height+6,None,None,None]
            rock[2] = [None,None,tower_height+5,tower_height+5,tower_height+5,None,None]
            rock[3] = [None,None,None,tower_height+4,None,None,None]
        #   ..#
        #   ..#
        #   ###
        elif(i % 5 == 2):
            rock[1] = [None,None,None,None,tower_height+6,None,None]
            rock[2] = [None,None,None,None,tower_height+5,None,None]
            rock[3] = [None,None,tower_height+4,tower_height+4,tower_height+4,None,None]
        #   #
        #   #
        #   #
        #   #
        elif(i % 5 == 3):
            rock[0] = [None,None,tower_height+7,None,None,None,None]
            rock[1] = [None,None,tower_height+6,None,None,None,None]
            rock[2] = [None,None,tower_height+5,None,None,None,None]
            rock[3] = [None,None,tower_height+4,None,None,None,None]
        #   ##
        #   ##
        else:
            rock[2] = [None,None,tower_height+5,tower_height+5,None,None,None]
            rock[3] = [None,None,tower_height+4,tower_height+4,None,None,None]
        
        step = True
        while(step):
            jet_dir = jets[jet % len(jets)] # determine if we should jet right or left

            # check if we can jet
            can_jet = True
            for j in range(len(rock)):
                # Check boundaries. If jet_dir = 1 (moving right), check last item in row (-1). Otherwise, check first item (0)
                if rock[j][-1 if jet_dir == 1 else 0] != None: can_jet = False 
                # Need to make sure we don't run into any existing rock structures
                for k in range(len(rock[j])): 
                    if can_jet and rock[j][k] != None and rock[j][k] in tower and tower[rock[j][k]][k+jet_dir]: can_jet = False
            
            if can_jet:
                # move left or right
                for j in range(len(rock)):
                    if jet_dir == 1: rock[j] = [None] + rock[j][:-1]
                    else: rock[j] = rock[j] = rock[j][1:] + [None]
            
            jet += 1                

            # check if we can move down
            for j in range(len(rock)):
                for k in range(len(rock[j])):                    
                    if step and rock[j][k] != None and rock[j][k]-1 in tower and tower[rock[j][k]-1][k]: step = False
            
            if step:
                # move down
                for j in range(len(rock)):
                    for k in range(len(rock[j])):
                        if rock[j][k] != None: rock[j][k] = rock[j][k] - 1
        
        #print_tower(tower)
        # update tower positions based on final resting place of the rock
        for j in range(len(rock)):
            for k in range(len(rock[j])):
                if rock[j][k] != None: 
                    if rock[j][k] not in tower: tower[rock[j][k]] = [False]*7
                    tower[rock[j][k]][k] = True
        
        # to save space, remove any rows lower than the highest fully filled row
        found = False
        for row_num,row in sorted(tower.items(), reverse=True):
            if found:
                del tower[row_num]
                continue
            if all(itm is True for itm in row):
                found = True

    # print_tower(tower)
    return max(tower)      

def build_tower_pt2(FINGERPRINT_ROWS):    
    # We only need to keep track of the top X rows of the tower, where X is distance from the last fully filled in row to the top of the tower
    # To start, define the floor at level 0
    tower = {0: [True]*7}
    jet = 0 # jet index
    i = 0 # rock # index

    # we're looking for where the cycle repeats; that is, where we encounter the same "fingerprint"
    cycle_complete = False

    # Our fingerprint/signature for a particular tower state will just be the heights of the top X rows (+ jet index and rock shape index)
    # This isn't necessary 100% correct as it doesn't account for overhangs that could be filled via lateral jets, but seems to work
    fingerprint = ([],-1,-1) #[[None]*7]*FINGERPRINT_ROWS 
    fingerprint_set = False
    fingerprint_start_height = -1
    fingerprint_start_rock = -1
    print("Finding cycle repetition...")
    while(not cycle_complete):
        rock = [[None]*7]*4 # rock is at most 4 rows high

        tower_height = max(tower)
        #    ####
        if(i % 5 == 0):
            rock[3] = [None,None,tower_height+4,tower_height+4,tower_height+4,tower_height+4,None]
        #    .#.
        #    ###
        #    .#.
        elif(i % 5 == 1):
            rock[1] = [None,None,None,tower_height+6,None,None,None]
            rock[2] = [None,None,tower_height+5,tower_height+5,tower_height+5,None,None]
            rock[3] = [None,None,None,tower_height+4,None,None,None]
        #   ..#
        #   ..#
        #   ###
        elif(i % 5 == 2):
            rock[1] = [None,None,None,None,tower_height+6,None,None]
            rock[2] = [None,None,None,None,tower_height+5,None,None]
            rock[3] = [None,None,tower_height+4,tower_height+4,tower_height+4,None,None]
        #   #
        #   #
        #   #
        #   #
        elif(i % 5 == 3):
            rock[0] = [None,None,tower_height+7,None,None,None,None]
            rock[1] = [None,None,tower_height+6,None,None,None,None]
            rock[2] = [None,None,tower_height+5,None,None,None,None]
            rock[3] = [None,None,tower_height+4,None,None,None,None]
        #   ##
        #   ##
        else:
            rock[2] = [None,None,tower_height+5,tower_height+5,None,None,None]
            rock[3] = [None,None,tower_height+4,tower_height+4,None,None,None]
        
        step = True
        while(step):
            jet_dir = jets[jet % len(jets)] # determine if we should jet right or left

            # check if we can jet
            can_jet = True
            for j in range(len(rock)):
                # Check boundaries. If jet_dir = 1 (moving right), check last item in row (-1). Otherwise, check first item (0)
                if rock[j][-1 if jet_dir == 1 else 0] != None: can_jet = False 
                # Need to make sure we don't run into any existing rock structures
                for k in range(len(rock[j])): 
                    if can_jet and rock[j][k] != None and rock[j][k] in tower and tower[rock[j][k]][k+jet_dir]: can_jet = False
            
            if can_jet:
                # move left or right
                for j in range(len(rock)):
                    if jet_dir == 1: rock[j] = [None] + rock[j][:-1]
                    else: rock[j] = rock[j] = rock[j][1:] + [None]
            
            jet += 1                

            # check if we can move down
            for j in range(len(rock)):
                for k in range(len(rock[j])):                    
                    if step and rock[j][k] != None and rock[j][k]-1 in tower and tower[rock[j][k]-1][k]: step = False
            
            if step:
                # move down
                for j in range(len(rock)):
                    for k in range(len(rock[j])):
                        if rock[j][k] != None: rock[j][k] = rock[j][k] - 1
        
        # update tower positions based on final resting place of the rock
        for j in range(len(rock)):
            for k in range(len(rock[j])):
                if rock[j][k] != None: 
                    if rock[j][k] not in tower: tower[rock[j][k]] = [False]*7
                    tower[rock[j][k]][k] = True
        
        # to save space, remove any rows lower than the highest fully filled row
        found = False
        for row_num,row in sorted(tower.items(), reverse=True):
            if found and row_num < max(tower) - FINGERPRINT_ROWS:
                del tower[row_num]
                continue
            if all(itm is True for itm in row):
                found = True

        i += 1 # increment rock counter
        tower_height = max(tower)

        # Test if our cycle has repeated
        #if all(itm is True for itm in tower[max(tower)]): # check if the top row is full (This was taking way too long on my i5 laptop)
        pattern_match = True
        if fingerprint_set and jet % len(jets) == fingerprint[1] and i % 5 == fingerprint[2]:
            for j,row in enumerate(fingerprint[0]):
                if row != tower[tower_height - j]:
                    pattern_match = False
                    break

            if pattern_match: # check if jet and rock type cycles are complete
                cycle_complete = True

        # After our first FINGERPRINT_ROWS, set our fingerprint
        if (not fingerprint_set and tower_height >= FINGERPRINT_ROWS and tower_height >= 5000):
            fingerprint_start_height = tower_height
            fingerprint_start_rock = i
            top_rows = []
            for j in range(tower_height,tower_height - FINGERPRINT_ROWS,-1):
            #for row_num,row in sorted(tower.items(), reverse=True):
                top_rows.append(tower[j])
            fingerprint = (top_rows, jet % len(jets), i % 5)
            fingerprint_set = True        


        if(i % 10000 == 0): print("Rock {i} (max height: {height}, jet: {jet_num}, shape: {shape_num})".format(i=i, height=tower_height, jet_num = jet % len(jets), shape_num = i % 5))

    # return cycle height, cycle # of rocks, cycle starting height, cycle starting rock #, and the actual fingerprint we used
    return (tower_height - fingerprint_start_height, i - fingerprint_start_rock, fingerprint_start_height, fingerprint_start_rock, fingerprint) 

def print_tower(tower):
    print()
    for row_num,row in sorted(tower.items(), reverse=True):
        print(format(row_num, '04')+'|',end='')
        print(''.join('#' if itm else ' ' for itm in row) + '|')

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()
jets = [-1 if char == '<' else 1 for char in input[0]]
NUM_ROCKS = 2022

tower_height = build_tower(NUM_ROCKS)

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(tower_height)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

NUM_ROCKS = 1000000000000
FINGERPRINT_ROWS = 15
(cycle_height,cycle_rocks,fingerprint_start_height,fingerprint_start_rock,fingerprint) = build_tower_pt2(FINGERPRINT_ROWS) # this gives us # of rocks and tower height in a full cycle
num_cycles = (NUM_ROCKS - fingerprint_start_rock) // cycle_rocks
remainder = (NUM_ROCKS - fingerprint_start_rock) % cycle_rocks

# Build a tower with the remaining rocks and measure it's height (using pt1 function). 
# Make sure we use our cycle starting fingerprint (subtracting off the initial height of the fingerprint rows)
remainder_height = build_tower(remainder,fingerprint) - len(fingerprint[0])

tower_height = fingerprint_start_height + cycle_height*num_cycles + remainder_height

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(tower_height)))