# Advent of code Year 2022 Day 23 solution
# Author = Trent Huhn
# Date = December 2022

import time

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()


part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(None)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()



part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(None)))