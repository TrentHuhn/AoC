# Advent of code Year 2022 Day 4 solution
# Author = Trent Huhn
# Date = December 2022

import time

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    inputLines = input_file.read().split('\n')

print("Starting Part One")
start_time = time.time()

pairs = []
for i,line in enumerate(inputLines):
    pairs.append([]) # add new pair
    elf1 = line.split(',')[0]
    elf2 = line.split(',')[1]
    pairs[i].append([]) # for each pair, instantiate two list objects (for the two elves)
    pairs[i].append([])
    pairs[i][0].append(int(elf1.split('-')[0]))
    pairs[i][0].append(int(elf1.split('-')[1]))
    pairs[i][1].append(int(elf2.split('-')[0]))
    pairs[i][1].append(int(elf2.split('-')[1]))

numOverlappingPairs = 0
for i,pair in enumerate(pairs):
    if (pair[0][0] >= pair[1][0] and pair[0][1] <= pair[1][1]) \
        or (pair[1][0] >= pair[0][0] and pair[1][1] <= pair[0][1]): # test if either group falls completely within the other
        numOverlappingPairs+=1


part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(numOverlappingPairs)))

start_time = time.time()

numOverlappingPairs = 0
for i,pair in enumerate(pairs):
    if (pair[0][0] <= pair[1][1] and pair[0][1] >= pair[1][0]): # test if the start or end of elf 1 range overlaps with elf 2 range
        numOverlappingPairs+=1

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(numOverlappingPairs)))