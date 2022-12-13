# Advent of code Year 2022 Day 13 solution
# Author = Trent Huhn
# Date = December 2022

import time
from functools import cmp_to_key

# Recursive function to determine if a pair of messages are in the correct order
def check_vals(line1, line2):
    for i in range(len(line1)):
        # if we run out of items on the right side, then return False since we know list2 ran out of values
        if i >= len(line2): return False

        # check items at specified index
        if type(line1[i]) == list or type(line2[i]) == list: # if either item is a list, then recurse
                valid = check_vals(line1[i] if type(line1[i]) == list else [line1[i]], line2[i] if type(line2[i]) == list else [line2[i]]) 
                if valid != None: return valid # only return if we have a definitive True/False result
        else: # both items must be ints
           if line1[i] != line2[i]: return line1[i] < line2[i] # only return if one value is larger/smaller than the other 

    # if we make it here, that means left side has run out so we need to test the right side to see if there are more values or not
    return None if len(line1) == len(line2) else True
    
# Helper function to use when sorting a list of messages
def compare_vals(line1, line2):
    valid = check_vals(line1,line2) 
    if valid == True: return -1
    elif valid == False: return 1
    else: return 0


with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

# parse the input
pairs = []
for i in range(0,len(input),3):
    line1 = input[i]
    line2 = input[i+1] if i + 1 < len(input) else None
    if line1.isupper() or line1.islower() or line2.isupper() or line2.islower(): # check if we have any alpha characters
        print("Invalid line detected: {line}".format(line = line))
        continue

    pairs.append((eval(line1), eval(line2))) # use eval to convert string into python nested lists

valid_pairs = []
# loop through each pair of lines
for i in range(len(pairs)):
    line1 = pairs[i][0]
    line2 = pairs[i][1]
    if check_vals(line1, line2) != False: valid_pairs.append(i+1) # pair indices start at 1

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(sum(valid_pairs))))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

# loop through all lines
all_lines = [line for pair in pairs for line in pair] # flatten pairs into single list
all_lines.append([[6]])
all_lines.append([[2]]) # add divider packets

all_lines.sort(key=cmp_to_key(compare_vals)) # sort list using our custom comparison function
index1 = all_lines.index([[2]]) + 1 # find the index of each divider packer
index2 = all_lines.index([[6]]) + 1
print(*all_lines, sep="\n")

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(index1*index2)))