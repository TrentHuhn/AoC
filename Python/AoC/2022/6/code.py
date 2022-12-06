# Advent of code Year 2022 Day 6 solution
# Author = Trent Huhn
# Date = December 2022

import time, sys

def checkIfDuplicates(list):
    mySet = set()
    for elem in list:
        if elem in mySet:
            return True
        else:
            mySet.add(elem)
    return False

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read()


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

#input = "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw" # testing (should be 7)
datastream = [*input]

if len(datastream) < 4:
    print("Datastream must contain at least 4 signals")
    sys.exit()

nonRepeatingIndex = -1
for i in range(len(datastream)-3):
    if not checkIfDuplicates(datastream[i:i+4]):
        nonRepeatingIndex = i+4
        break

if nonRepeatingIndex == -1:
    print("No non-duplicating marker found")    

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(nonRepeatingIndex)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

#input = "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg" # testing (should be 29)
datastream = [*input]

nonRepeatingIndex = -1
for i in range(len(datastream)-3):
    if not checkIfDuplicates(datastream[i:i+14]):
        nonRepeatingIndex = i+14
        break

if nonRepeatingIndex == -1:
    print("No non-duplicating message marker found")    


part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(nonRepeatingIndex)))