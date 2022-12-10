# Advent of code Year 2022 Day 9 solution
# Author = Trent Huhn
# Date = December 2022

import time

def updatePosition(curHeadPos,curTailPos,instr,visitedCoords):
    delta_x = 0
    delta_y = 0
    if instr == 'R':
        delta_x = 1
    elif instr == 'L':
        delta_x = -1
    elif instr == 'U':
        delta_y = 1
    elif instr == 'D':
        delta_y = -1
    else:
        print("Unexpected input instruction {instr}".format(instr = instr))
        return

    newHeadPos = (curHeadPos[0] + delta_x, curHeadPos[1] + delta_y)

    # determine if the tail has to move by checking if the difference in any coordinate is more than 1
    x_diff = newHeadPos[0] - curTailPos[0]
    y_diff = newHeadPos[1] - curTailPos[1]

    new_x = curTailPos[0]
    new_y = curTailPos[1]
    if x_diff != 0 and (abs(y_diff) > 1 or abs(x_diff) > 1): # check if x-coord is different and at least one coord is more than 1 unit away
       new_x = new_x + (1 if x_diff > 0 else -1)
    if y_diff != 0 and (abs(y_diff) > 1 or abs(x_diff) > 1): # check if y-coord is different and at least one coord is more than 1 unit away
       new_y = new_y + (1 if y_diff > 0 else -1)

    # update list of visited coordinates
    newTailPos = (new_x, new_y)
    if newTailPos not in visitedCoords: visitedCoords.append(newTailPos)

    return (newHeadPos, newTailPos)

def updatePositionPt2(curPos,instr,visitedCoords):
    delta_x = 0
    delta_y = 0
    newPos = curPos

    if instr == 'R':
        delta_x = 1
    elif instr == 'L':
        delta_x = -1
    elif instr == 'U':
        delta_y = 1
    elif instr == 'D':
        delta_y = -1
    else:
        print("Unexpected input instruction {instr}".format(instr = instr))
        return

    # update position of head
    newPos[0] = (curPos[0][0] + delta_x, curPos[0][1] + delta_y)

    # now loop through each remaining knot and determine if/how it moves
    for i in range(1,len(curPos)):
        # determine if the knot has to move by checking if the difference between it and the previous knot in any coordinate is more than 1
        x_diff = newPos[i-1][0] - curPos[i][0]
        y_diff = newPos[i-1][1] - curPos[i][1]

        new_x = curPos[i][0]
        new_y = curPos[i][1]
        if x_diff != 0 and (abs(y_diff) > 1 or abs(x_diff) > 1): # check if x-coord is different and at least one coord is more than 1 unit away
           new_x = new_x + (1 if x_diff > 0 else -1)
        if y_diff != 0 and (abs(y_diff) > 1 or abs(x_diff) > 1): # check if y-coord is different and at least one coord is more than 1 unit away
           new_y = new_y + (1 if y_diff > 0 else -1)

        newPos[i] = (new_x, new_y)

    # update list of visited coordinates
    if newPos[-1] not in visitedCoords: visitedCoords.append(newPos[-1])

    return newPos
    

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()
visitedCoords = []
curHeadPos = (0,0)
curTailPos = (0,0)
for i in range(0,len(input)): # loop through each instruction
    line = input[i].split(' ')
    instr = line[0]
    steps = int(line[1])
    for j in range(0,steps):
        (curHeadPos, curTailPos) = updatePosition(curHeadPos, curTailPos,instr,visitedCoords)


part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(len(visitedCoords))))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

visitedCoords = []
curPos = []
NUM_KNOTS = 10
# use a tuple to represent current position of all 10 knots
for i in range(0,NUM_KNOTS):
    curPos.append((0,0))

for i in range(0,len(input)): # loop through each instruction
    line = input[i].split(' ')
    instr = line[0]
    steps = int(line[1])
    for j in range(0,steps):
        curPos = updatePositionPt2(curPos,instr,visitedCoords) # update position of each knot


part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(len(visitedCoords))))