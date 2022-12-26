# Advent of code Year 2022 Day 24 solution
# Author = Trent Huhn
# Date = December 2022

import time
import math
import os

def bfs(walls, blizzards, node, target):
    minute = 0
    lcm = math.lcm(max(walls,key=lambda pt:pt[0])[0]-2,max(walls,key=lambda pt:pt[1])[1]-2)
    cur_nodes = {node} 
    blizz_nodes = [b[0] for b in blizzards]
    
    while target not in cur_nodes:
        # calculate new blizzard positions         
        next_blizzards = set()
        for b in blizzards:
            x = b[0][0]
            y = b[0][1]
            dx = b[1][0]
            dy = b[1][1]
            x += dx
            y += dy
            if (x,y) in walls:
                x -= dx
                y -= dy
                while (x,y) not in walls:
                    x -= dx
                    y -= dy
                x += dx
                y += dy
            next_blizzards.add(((x,y),(dx,dy)))            
        
        blizzards = next_blizzards
        blizz_nodes = [b[0] for b in blizzards]
        
        next_nodes = set()
        for node in cur_nodes:
            neighbors = [(node[0] - 1, node[1]), (node[0] + 1, node[1]), (node[0], node[1] - 1), (node[0], node[1] + 1)]
            for neighbor in neighbors:
                # ensure that this neighbor is not a wall and does not currently have any blizzards occupying it
                if neighbor not in walls and neighbor not in blizz_nodes:                
                    next_nodes.add(neighbor)
         
            if node not in blizz_nodes: # stay at current node
                next_nodes.add(node)   

        #if minute % 25 == 0:
        #    print_valley(walls,blizzards,next_nodes,minute)

        cur_nodes = next_nodes
        minute += 1

    return (minute, blizzards) # back up one minute

def print_valley(walls,blizzards,possible_positions,minute):
    os.system('cls')
    print("Minute: " + str(minute))
    x_max = max(walls,key=lambda pt:pt[0])[0]
    y_max = max(walls,key=lambda pt:pt[1])[1]
    blizz_nodes = [bliz[0] for bliz in blizzards]

    for j in range(-1,y_max+1):
        for i in range(x_max+1):
            print('#' if (i,j) in walls else str(blizz_nodes.count((i,j))) if (i,j) in blizz_nodes else '+' if (i,j) in possible_positions else ' ',end='')
        print()
    
    time.sleep(0.1)

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

assert len(input) > 0, len(input[0] > 0)
grid = {}
y_max = len(input)
x_max = len(input[0])
blizzards = set()
walls = set()
for i in range(len(input)):
    line = input[i]
    for j in range(len(line)):
        if line[j] == '#': walls.add((j,i))
        elif line[j] == '<': blizzards.add(((j,i),(-1,0))) # 0 = moving west
        elif line[j] == '^': blizzards.add(((j,i),(0,-1))) # 1 = moving north
        elif line[j] == '>': blizzards.add(((j,i),(1,0))) # 2 = moving east
        elif line[j] == 'v': blizzards.add(((j,i),(0,1))) # 3 = moving south

# need to hard-code some boundaries around our start and end positions
walls.add((1,-1))
walls.add((x_max - 2, y_max))

# Driver Code
(pt1, blizzards_new) = bfs(walls, blizzards, (1,0), (x_max - 2, y_max - 1))

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(pt1)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

(pt2,blizzards_new) = bfs(walls, blizzards_new, (x_max - 2, y_max - 1), (1,0))
(pt3,blizzards_new) = bfs(walls, blizzards_new, (1,0), (x_max - 2, y_max - 1))

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(pt1+pt2+pt3)))