# Advent of code Year 2022 Day 1 solution
# Author = Trent Huhn
# Date = December 2022

import time
with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    #input = input_file.read()
    inputArray = input_file.readlines()

print("Starting Part One")
start_time = time.time()
    
elves = []
if len(inputArray) > 0:
   elves.append([])
elfNum = 0
maxElfCals = 0

calArray = list(map(lambda x: '' if not x.strip('\n').isnumeric() else int(x.strip('\n')), inputArray))
for i,line in enumerate(calArray):
    if line != "":        
        elves[elfNum].append(line)
    else:
        curSum = sum(elves[elfNum])
        if curSum > maxElfCals:
            maxElfCals = curSum
        elfNum += 1     
        elves.append([])

# account for final elf
curSum = sum(elves[elfNum])
if curSum > maxElfCals:
    maxElfCals = curSum

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(maxElfCals)))

start_time = time.time()


elves = []
if len(inputArray) > 0:
   elves.append([])
elfNum = 0
elfCals = []

for i,line2 in enumerate(calArray):
    if line2 != "":
        elves[elfNum].append(line2)
    else:
        curSum = sum(elves[elfNum])
        elfCals.append(curSum)
        elfNum += 1
        elves.append([])

# account for final elf
curSum = sum(elves[elfNum])
elfCals.append(curSum)
        
topThreeSum = 0
elfCals.sort(reverse=True)
if len(elfCals) >= 3:
    topThreeSum = elfCals[0] + elfCals[1] + elfCals[2]

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(topThreeSum)))