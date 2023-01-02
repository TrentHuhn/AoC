# Advent of code Year 2022 Day 22 solution
# Author = Trent Huhn
# Date = December 2022

import time
import re

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")

def add_edge(pos,add,edge):
    if pos not in edge.keys():
        edge[pos] = []
    edge[pos].append(add)

########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

board = {}
instr = []
for i in range(len(input)):
    if  len(input[i]) > 0 and input[i][0] in ['.','#',' ']:
        for j in range(len(input[i])):
            char = input[i][j]
            if char in ['.','#']: # don't add non-board spaces to our matrix
                board[(j,i)]= 0 if char == '.' else 1 if char == '#' else None
    else:
        #instr = [c for c in input[i]]
        instr = re.split('([RL])',input[i])

dirs = [(1,0),(0,1),(-1,0),(0,-1)] # possible directions

cur = min(list(filter(lambda elem: elem[0][1] == 0 and elem[1] == 0,board.items())))[0] # get first open space in the top row (y = 0)
dir = 0 # start facing right
for i in range(len(instr)):
    ins = instr[i]
    if ins.isnumeric():
        #(x_max, x_min),  (y_max, y_min) = [(max(x), min(x)) for x in zip(*board.keys())]
        for j in range(int(ins)):
            
            d = dirs[dir]
            #new = (cur[0] + delta[0] % x_max, cur[1] + delta[1] % y_max)

            x = cur[0]
            y = cur[1]
            dx = d[0]
            dy = d[1]
            x += dx
            y += dy
            if (x,y) not in board.keys():
                x -= dx
                y -= dy
                while (x,y) in board.keys():
                    x -= dx
                    y -= dy
                x += dx
                y += dy
            new = (x,y)

            assert new in board.keys(), "New coords not on board"
            if board[new] == 1: 
                continue # we hit a wall

            cur = new    
    else: # we need to turn
        assert ins in ['R','L'], "Invalid direction"
        if ins == 'R':
            dir = (dir + 1) % 4
        else:
            dir = (dir + 3) % 4

final = 1000 * (cur[1] + 1) + 4 * (cur[0] + 1) + dir
part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(final)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

# Need to define edge transition dict
# Each edge node contains:
#  - Tuple containing 1st choice and 2nd choice
#    - Each "choice" contains a new position and a new direction
EDGE_SIZE = 50
edge = {}
(x_max, x_min),  (y_max, y_min) = [(max(x), min(x)) for x in zip(*board.keys())]
for key,val in board.items():
    # Top of board
    if key[1] == 0:
        # Left side
        if key[0] >= 50 and key[0] < 100:
            add_edge((key[1] - 1, key[0]), ((0,y_max-(2*EDGE_SIZE - key[0])),0), edge)        
        if key[0] >= 100:
            add_edge((key[1] - 1, key[0]), ((y_max,key[0]-EDGE_SIZE),3), edge)

cur = min(list(filter(lambda elem: elem[0][1] == 0 and elem[1] == 0,board.items())))[0] # get first open space in the top row (y = 0)
dir = 0 # start facing right
for i in range(len(instr)):
    ins = instr[i]
    if ins.isnumeric():
        for j in range(int(ins)):            
            d = dirs[dir]
            x = cur[0] + d[0]
            y = cur[1] + d[1]
            nd = dir
            # Need to set up wrapping rules. This is all hard-coded for our specific cube pattern.
            if dir % 2 == 0: # If we're traveling east or west on our grid
                if 0 <= y < 50:
                    if x >= 150:
                        x = 99
                        y = 149 - y
                        nd = 2 # now heading west (<)
                    elif x < 50:
                        x = 0
                        y = 149 - y
                        nd = 0 # now heading east (>)
                elif 50 <= y < 100:
                    if x >= 100:
                        x = y + 50
                        y = 49
                        nd = 3 # now heading north (^)
                    elif x < 50:
                        x = y - 50
                        y = 100
                        nd = 1 # now heading south (v)
                elif 100 <= y < 150:
                    if x >= 100:
                        x = 149
                        y = 149 - y
                        nd = 2 # now heading west (<)
                    elif x < 0:
                        x = 50
                        y = 149 - y
                        nd = 0 # now heading east (>)
                elif 150 <= y < 200:
                    if x < 0:
                        x = y - 100
                        y = 0
                        nd = 1 # now heading south (V)
                    elif x >= 50:
                        x = y - 100
                        y = 149
                        nd = 3 # now heading north (^)
            else: # we're heading north or south
                if 0 <= x < 50:
                    if y < 100:
                        y = x + 50
                        x = 50
                        nd = 0 # now heading east (>)
                    elif y >= 200:
                        y = 0
                        x += 100
                        assert dir == 1, "Incorrect direction encountered"
                        # direction unchanged (still heading south)
                elif 50 <= x < 100:
                    if y < 0:
                        y = x + 100
                        x = 0
                        nd = 0 # now heading east (>)
                    elif y >= 150:
                        y = x + 100
                        x = 49
                        nd = 2 # now heading west (<)
                elif 100 <= x < 150:
                    if y < 0:
                        x -= 100
                        y = 199
                        assert dir == 3, "Incorrect direction encountered"
                        # direction unchanged (still heading north)
                    elif y >= 50:
                        y = x - 50
                        x = 99
                        nd = 2 # now heading west (<)
            
            new = (x, y)
            
            assert new in board.keys(), "New coords not on board"
            if board[new] == 1: 
                continue # we hit a wall
            
            # update our current position and direcdtion
            cur = new    
            dir = nd
    else: # we need to turn
        assert ins in ['R','L'], "Invalid direction"
        if ins == 'R':
            dir = (dir + 1) % 4
        else:
            dir = (dir + 3) % 4

final = 1000 * (cur[1] + 1) + 4 * (cur[0] + 1) + dir

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(final)))