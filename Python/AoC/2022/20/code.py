# Advent of code Year 2022 Day 20 solution
# Author = Trent Huhn
# Date = December 2022

import time

def mix(nums, indices, num_mixes):
    for i in range(len(indices) * num_mixes):
        idx = i % len(indices)
        if idx == 0: print("Mix: " + str((i // 5000)+1))
        # get the current index of the value we're on (i)
        j = indices.index(idx)
        # remove this value from it's current position
        indices.pop(j) 
        # and insert it where it's supposed to go (note that len(indices) will be 1 less than the total # of values in our list because we just popped an item)
        indices.insert((j + nums[idx]) % len(indices), idx) 

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

#dict = {}
VALS = [1000,2000,3000]
nums = []
indices = []
for i in range(len(input)):
    nums.append(int(input[i]))
    indices.append(i)

# Start mixing
mix(nums, indices, 1)

vals = []
zero_pos = indices.index(nums.index(0))
for i in range(len(VALS)):
    pos = (VALS[i] + zero_pos) % len(indices)
    val = nums[indices[pos]]
    vals.append(val)
total = sum(vals)

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(total)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

DECRYPT_KEY = 811589153
NUM_MIXES = 10

nums = []
indices = []
for i in range(len(input)):
    nums.append(int(input[i]) * DECRYPT_KEY)
    indices.append(i)

# Start mixing
mix(nums,indices,NUM_MIXES)

vals = []
zero_pos = indices.index(nums.index(0))
for i in range(len(VALS)):
    pos = (VALS[i] + zero_pos) % len(indices)
    val = nums[indices[pos]]
    vals.append(val)
total = sum(vals)

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(total)))