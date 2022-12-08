# Advent of code Year 2022 Day 8 solution
# Author = Trent Huhn
# Date = December 2022

import time

def isVisible(yLoc,xLoc,forest):
    curHeight = forest[yLoc][xLoc]
    visible = 0
    if yLoc in (0,len(forest)-1) or xLoc in (0,len(forest[yLoc])-1): # test if we're on an edge
        return 1
    
    # test to the north
    for i in range(yLoc-1,-2,-1):
        if i == -1: return 1 # we've reached the edge, so we know this tree is visible from the north
        if forest[i][xLoc] >= curHeight: break # we found a taller tree
     
    # test to the east
    for j in range(xLoc+1,len(forest[yLoc])+1):
        if j == len(forest[yLoc]): return 1 # we've reached the edge, so we know this tree is visible from the north
        if forest[yLoc][j] >= curHeight: break # we found a taller tree  
        
    # test to the south
    for i in range(yLoc+1,len(forest)+1):
        if i == len(forest): return 1 # we've reached the edge, so we know this tree is visible from the north
        if forest[i][xLoc] >= curHeight: break # we found a taller tree

    # test to the west
    for j in range(xLoc-1,-2,-1):
        if j == -1: return 1 # we've reached the edge, so we know this tree is visible from the north
        if forest[yLoc][j] >= curHeight: break # we found a taller tree  

    return visible

def computeScenicScore(yLoc,xLoc,forest):
    curHeight = forest[yLoc][xLoc]
    score = 0
    
    north,east,south,west = 0,0,0,0

    # test to the north
    for i in range(yLoc-1,-1,-1):
        north += 1
        if i == -1 or forest[i][xLoc] >= curHeight: break # we found a taller tree or reached the edge
     
    # test to the east
    for j in range(xLoc+1,len(forest[yLoc])):
        east += 1
        if j == len(forest[yLoc]) or forest[yLoc][j] >= curHeight: break # we found a taller tree or reached the edge 
        
    # test to the south
    for i in range(yLoc+1,len(forest)):
        south += 1
        if i == len(forest) or forest[i][xLoc] >= curHeight: break # we found a taller tree or reached the edge 

    # test to the west
    for j in range(xLoc-1,-1,-1):
        west += 1
        if j == -1 or forest[yLoc][j] >= curHeight: break # we found a taller tree or reached the edge 

    return north*east*south*west

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

forest = []
numVisible=0

# set up 2D array
for i in range(0,len(input)):
    line = input[i]
    forest.append([])
    for j in range(0,len(line)):
        forest[i].append([])
        forest[i][j] = int(line[j])

# traverse array and test each tree
for i in range(0,len(forest)):
    for j in range(0,len(forest[i])):
        numVisible += isVisible(i,j,forest)

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(numVisible)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

maxScenicScore = 0
# traverse array and compute each tree's scenic score
for i in range(0,len(forest)):
    for j in range(0,len(forest[i])):
        score = computeScenicScore(i,j,forest)
        if score > maxScenicScore: maxScenicScore = score

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(maxScenicScore)))