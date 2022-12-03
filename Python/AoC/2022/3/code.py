# Advent of code Year 2022 Day 3 solution
# Author = Trent Huhn
# Date = December 2022

import time

class Rucksack:
    def __init__(self) -> None:
        self.compartment1 = []
        self.compartment2 = []
        self.commonType = None

def getPriority(char):
    return ord(char)-96 if char.islower() else ord(char)-38

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    inputArray = input_file.read().split('\n')

print("Starting Part One")
start_time = time.time()

totalPriority = 0
rucksacks = []
for i,line in enumerate(inputArray): # iterate through each line (rucksack)
    rucksacks.append(Rucksack())
    for j,char in enumerate(list(line)): # break string into a list of chars
        if j<len(line)/2:
            rucksacks[i].compartment1.append(char)
        else:
            rucksacks[i].compartment2.append(char)
            if char in rucksacks[i].compartment1 and rucksacks[i].commonType == None: # check if there's overlapping item types and we haven't already found the common item type
                 totalPriority += getPriority(char)
                 rucksacks[i].commonType = char
    


part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(totalPriority)))

start_time = time.time()
totalPriority = 0
#rucksacks = []
curGroup = []
for i,line in enumerate(inputArray): # iterate through each line (rucksack)
    rucksack = Rucksack()
    if i % 3 == 0:
        curGroup = [] # reset the current elf group every 3 lines

    for j,char in enumerate(list(line)): # break string into a list of chars        
        rucksack.compartment1.append(char) # we don't care about compartments in part two, just put all items into compartment 1

    #rucksacks.append(rucksack)
    curGroup.append(rucksack)
    if len(curGroup) == 3: # once we have the full invenetories of the current group of three elves, check for overlapping item types 
        for j,char in enumerate(rucksack.compartment1):
            if char in curGroup[0].compartment1 and char in curGroup[1].compartment1 and rucksack.commonType == None: # check if there's overlapping item types and we haven't already found the common item type
                    totalPriority += getPriority(char)
                    rucksack.commonType = char

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(totalPriority)))

