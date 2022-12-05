# Advent of code Year 2022 Day 5 solution
# Author = Trent Huhn
# Date = December 2022

import time, re

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    lines = input_file.read().split('\n')

########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

numStacks = 9
stacks = []
# set up initial stack configuration
for i in range(7,-1,-1):    
    for j in range(0,numStacks):
        if len(stacks) < numStacks: stacks.append([])
        curCrate = lines[i][4*j+1]
        if curCrate != ' ': stacks[j].append(lines[i][4*j+1])

# iterate through all moves
for i in range(10,len(lines)):
    inst = re.split('\D+',lines[i]) # split on non-digits
    numMoved = int(inst[1])
    fromStack = int(inst[2]) - 1
    toStack = int(inst[3]) - 1
    for j in range(0,numMoved):
        c = stacks[fromStack].pop() # remove top crate
        stacks[toStack].append(c) # and add to destination stack

# get final configuration
finalConfig = ''
for i in range(0,numStacks):
    finalConfig = finalConfig + stacks[i].pop()

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(finalConfig)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

stacks = []
# set up initial stack configuration
for i in range(7,-1,-1):    
    for j in range(0,numStacks):
        if len(stacks) < numStacks: stacks.append([])
        curCrate = lines[i][4*j+1]
        if curCrate != ' ': stacks[j].append(lines[i][4*j+1])

# iterate through all moves
for i in range(10,len(lines)):
    inst = re.split('\D+',lines[i]) # split on non-digits
    numMoved = int(inst[1])
    fromStack = int(inst[2]) - 1
    toStack = int(inst[3]) - 1
    tempStack = []
    for j in range(0,numMoved):
        tempStack.append(stacks[fromStack].pop()) # remove top crate and add to temp stack

    tempStack.reverse() # reverse the temp stack
    stacks[toStack] = stacks[toStack] + tempStack # concatenate original stack with the reversed stack

# get final configuration
finalConfig = ''
for i in range(0,numStacks):
    finalConfig = finalConfig + stacks[i].pop()

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(finalConfig)))