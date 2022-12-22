# Advent of code Year 2022 Day 18 solution
# Author = Trent Huhn
# Date = December 2022

import time


with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

pts = [tuple(map(int,ln.split(','))) for ln in input]

surface_area = 0
for i in range(len(pts)):
    pt = pts[i]
    num_neighbors = 0
    check = [(1,0,0),(-1,0,0),(0,1,0),(0,-1,0),(0,0,1),(0,0,-1)]
    for j in range(len(check)):
        if (pt[0]+check[j][0],pt[1]+check[j][1],pt[2]+check[j][2]) in pts: num_neighbors += 1

    surface_area += 6 - num_neighbors

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(surface_area)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

# set up 3d matrix
# (need to extend range by 2 to ensure we have buffer space around edge of droplet)
matrix = [[[None for i in range(max(pts,key=lambda pt:pt[2])[2]+2)] for j in range(max(pts,key=lambda pt:pt[1])[1]+2)] for k in range(max(pts,key=lambda pt:pt[0])[0]+2)]

# place our blocks in the matrix
for i in range(len(pts)):
    pt = pts[i]
    matrix[pt[0]][pt[1]][pt[2]] = 1

nodes = []
nodes.append((0,0,0)) # starting position
while(len(nodes) > 0):
    (x,y,z) = nodes.pop()
    
    # check point validity (making sure to only update empty nodes)
    if x < 0 or x >= len(matrix) or y < 0 or y >= len(matrix[0]) or z < 0 or z >= len(matrix[0][0]) or matrix[x][y][z] != None:
        continue

    matrix[x][y][z] = 2
    # attempt to fill the neighboring positions
    nodes.append((x+1, y, z))
    nodes.append((x-1, y, z))
    nodes.append((x, y+1, z))
    nodes.append((x, y-1, z))
    nodes.append((x, y, z+1))
    nodes.append((x, y, z-1))

surface_area = 0
# now check to see how many of our points border with our flooded matrix
for i in range(len(pts)):
    pt = pts[i]
    check = [(1,0,0),(-1,0,0),(0,1,0),(0,-1,0),(0,0,1),(0,0,-1)]
    for j in range(len(check)):        
        x = pt[0]+check[j][0]
        y = pt[1]+check[j][1]
        z = pt[2]+check[j][2]
        if x == -1 or y == -1 or z == -1: # means the point is on the edge of our matrix (we can't have negative indices)
            surface_area += 1
            continue
        if x < 0 or x >= len(matrix) or y < 0 or y >= len(matrix[0]) or z < 0 or z >= len(matrix[0][0]): continue
        if matrix[x][y][z] == 2: surface_area += 1


part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(surface_area)))