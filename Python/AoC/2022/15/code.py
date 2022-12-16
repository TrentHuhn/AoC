# Advent of code Year 2022 Day 15 solution
# Author = Trent Huhn
# Date = December 2022

import time
from itertools import product

def draw_grid(grid):
    print()
    for j in range(y_min,y_max+1):
        print(f"{j:02}",end='')
        for i in range(x_min,x_max+1):
            print('.' if grid[(i,j)] == None else '#',end='')
        print()

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

Y_ROW = 2000000
RUN_PART_ONE = False
DIM_MIN = 0
DIM_MAX = 4000000

# parse input
sensor_dict = {}
for i in range(len(input)):
    line = ''.join(char for char in input[i] if char.isdigit() or char in ['-',' ']) # keep just digits and spaces
    line = ' '.join(line.split()).split(' ') # remove duplicate spaces and then split into list
    sensor = (int(line[0]),int(line[1]))
    beacon = (int(line[2]),int(line[3]))
    sensor_dict[sensor] = beacon

# compute bounds of our grid (not needed)
#x_max = max(max(sensor_dict)[0],max(sensor_dict.items(), key=lambda k: k[1][0])[1][0])
#y_max = max(max(sensor_dict, key=lambda x: x[1])[1],max(sensor_dict.items(), key=lambda k: k[1][1])[1][1])

#x_min = min(min(sensor_dict)[0],min(sensor_dict.items(), key=lambda k: k[1][0])[1][0])
#y_min = min(min(sensor_dict, key=lambda x: x[1])[1],min(sensor_dict.items(), key=lambda k: k[1][1])[1][1])    

count=0
if RUN_PART_ONE:
    #grid = dict([((i,j),None) for i in range(x_min,x_max+1) for j in range(Y_ROW,Y_ROW+1)])
    grid = {}
    for sensor,beacon in sensor_dict.items():
        print("{sensor}: {beacon}".format(sensor = sensor, beacon = beacon))
        # check if sensor and/or beacon are in Y_ROW
        if sensor[1] == Y_ROW: grid[sensor[0]] = 0
        if beacon[1] == Y_ROW: grid[beacon[0]] = -1
        x_dist = abs(sensor[0] - beacon[0])  
        y_dist = abs(sensor[1] - beacon[1])
        dist = x_dist + y_dist
        for i in range(sensor[0]-dist,sensor[0]+dist+1): 
            if abs(sensor[0] - i) + abs(sensor[1] - Y_ROW) <= dist and i not in grid.keys(): # check if this point's distance to the sensor is less than the distance from the sensor to the beacon
                grid[i]=1
                count += 1



part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(count)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

# I'll be honest, don't really understand how the below part works.
# Credit to 4HbQ on on reddit (https://www.reddit.com/r/adventofcode/comments/zmcn64/comment/j0bqznu/?utm_source=reddit&utm_medium=web2x&context=3)
sensor_data = [(sx, sy, abs(sx - bx) + abs(sy - by)) for (sx, sy), (bx, by) in sensor_dict.items()]

f = lambda x,y,d,p,q,r: ((p+q+r+x-y-d)//2, (p+q+r-x+y+d)//2+1)

x_final = None
y_final = None
for X, Y in [f(*a,*b) for a in sensor_data for b in sensor_data]:
    if DIM_MIN<X<DIM_MAX and DIM_MIN<Y<DIM_MAX and all(abs(X - sx) + abs(Y - sy) > d for sx,sy,d in sensor_data):
        x_final = X
        y_final = Y

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(x_final * 4000000 + y_final)))