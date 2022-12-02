# Advent of code Year 2022 Day 2 solution
# Author = Trent Huhn
# Date = December 2022

import time

def computeScore(opp, mine):
    score = 0
    score += (1 if mine == 'X' else 2 if mine == 'Y' else 3)
    if (mine == 'X' and opp == 'C') or (mine == 'Y' and opp == 'A') or (mine == 'Z' and opp == 'B'):
        score += 6
    elif (mine == 'X' and opp == 'A') or (mine == 'Y' and opp == 'B') or (mine == 'Z' and opp == 'C'):
        score += 3    
    
    return score

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    inputArray = input_file.read().split('\n')

print("Starting Part One")
start_time = time.time()
  
totalScore = 0
for i,line in enumerate(inputArray):
    theirMove = line.split(' ')[0]
    myMove = line.split(' ')[1]
    totalScore += computeScore(theirMove, myMove)


part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(totalScore)))

start_time = time.time()

totalScore = 0
for i,line in enumerate(inputArray):
    theirMove = line.split(' ')[0]
    outcome = line.split(' ')[1]
    myMove = None
    if outcome == 'X': # need to lose
        myMove = 'X' if theirMove == 'B' else 'Y' if theirMove == 'C' else 'Z'
    elif outcome == 'Y': # need to draw
        myMove = 'X' if theirMove == 'A' else 'Y' if theirMove == 'B' else 'Z'
    else: # need to win
        myMove = 'X' if theirMove == 'C' else 'Y' if theirMove == 'A' else 'Z'

    totalScore += computeScore(theirMove, myMove)


part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(totalScore)))

