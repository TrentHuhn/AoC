# Advent of code Year 2022 Day 14 solution
# Author = Trent Huhn
# Date = December 2022

import time

# method to compute next location of a unit of sand
# note that locaiton is specified as (x,y) but cave is indexed as cave[y][x]
def compute_next_loc(location,cave):
    # test if we're at the bottom of our cave grid
    if location[1] == len(cave) - 1: return (location, False, False) # set falling and more_sand to False

    new_location = location
    falling = True
    # test if tile below is open
    if cave[location[1]+1][location[0]] == None:
        new_location = (location[0],location[1]+1)
    elif cave[location[1]+1][location[0]-1] == None:
        new_location = (location[0]-1,location[1]+1)
    elif cave[location[1]+1][location[0]+1] == None:
        new_location = (location[0]+1,location[1]+1)
    else:
        falling = False

    return (new_location, falling, True)

# helper function to draw the current state of the cave system
def draw_cave(cave,dims):
    print()
    for i in range(dims[2],dims[3]+1):
        line = cave[i]
        print(format(i, '03') + ' ',end='')
        for j in range(dims[0],dims[1]+1):
            item = line[j]
            print('#' if item == 1 else '+' if item == 0 else '.',end='')
        print('\n')

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

# parse input file into points/lines
lines = []
for i,line_text in enumerate(input):
    coord_text = line_text.split(' -> ')
    coords = []
    for j,coord in enumerate(coord_text):
        pts = coord.split(',')
        coords.append((int(pts[0]),int(pts[1])))
    lines.append(coords)

# find max/min x and y coords
max_x = max(coord[0] for line in lines for coord in line) 
min_x = min(coord[0] for line in lines for coord in line) 
max_y = max(coord[1] for line in lines for coord in line) 
min_y = min(coord[1] for line in lines for coord in line) 
dims = (min_x,max_x,min_y,max_y)
cave = [[None] * (max_x+1) for i in range(max_y+1)]

# build cave system
for i in range(len(lines)):
    line = lines[i]
    for j in range(len(line)-1): # don't need to process the last coord of each line
        coord = line[j]
        next_coord = line[j+1]
        if coord[0] == next_coord[0]: # x-coords are equal, this is a vertical line segment
            for k in range(min(coord[1],next_coord[1]),max(coord[1],next_coord[1])+1):
                cave[k][coord[0]] = 1 # place a rock at this coord
        elif coord[1] == next_coord[1]: # y-coords are equal, this is a horizontal line segment            
            for k in range(min(coord[0],next_coord[0]),max(coord[0],next_coord[0])+1):
                cave[coord[1]][k] = 1 # place a rock at this coord
        else:
            print("Error: Non-vertical or non-horizontal line found")
            break

draw_cave(cave,dims)

# simulate sand
more_sand = True
num_sand = 0
while more_sand:
    cur_location = (500,0)
    falling = True
    while falling:
        (cur_location,falling,more_sand) = compute_next_loc(cur_location,cave)

    if more_sand:
        cave[cur_location[1]][cur_location[0]] = 0 # update cave with ending position of sand
        num_sand += 1

#draw_cave(cave,dims)
part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(num_sand)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

# build cave system with floor
dims = (0,2*max_x,0,max_y+2)
cave = [[None] * (dims[1]+1) for i in range(dims[3]+1)]

# add floor
for i in range(len(cave[dims[3]])): 
    cave[dims[3]][i] = 1

for i in range(len(lines)):
    line = lines[i]
    for j in range(len(line)-1): # don't need to process the last coord of each line
        coord = line[j]
        next_coord = line[j+1]
        if coord[0] == next_coord[0]: # x-coords are equal, this is a vertical line segment
            for k in range(min(coord[1],next_coord[1]),max(coord[1],next_coord[1])+1):
                cave[k][coord[0]] = 1 # place a rock at this coord
        elif coord[1] == next_coord[1]: # y-coords are equal, this is a horizontal line segment            
            for k in range(min(coord[0],next_coord[0]),max(coord[0],next_coord[0])+1):
                cave[coord[1]][k] = 1 # place a rock at this coord
        else:
            print("Error: Non-vertical or non-horizontal line found")
            break

# simulate sand
more_sand = True
num_sand = 0
while more_sand:
    cur_location = (500,0)
    falling = True
    while falling:
        (cur_location,falling,more_sand) = compute_next_loc(cur_location,cave)
        
    if cur_location == (500,0): more_sand = False
    if more_sand:
        cave[cur_location[1]][cur_location[0]] = 0 # update cave with ending position of sand

draw_cave(cave,(300,800,0,max_y+2))

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(num_sand)))