# Advent of code Year 2022 Day 25 solution
# Author = Trent Huhn
# Date = December 2022

import time
import math

def convert_snafu(num):
    sum = 0
    for i in range(len(num)):
        s = num[-(i+1)]
        d = None
        if s == '=': d = -2
        elif s == '-': d = -1
        else: d = int(s)

        sum += (5**i * d)
    return sum

def convert_decimal(num):
    dec = str()
    p = int((math.log(abs(num)) // math.log(5)) + 1)
    base = 0
    for i in range(p,-1,-1):
        half = (5**i) // 2
        assert 2*(5**i) + base + half >= num >= base - 2*(5**i) - half, "Invalid power"
        if base + 2*(5**i) + half >= num >= base + (2*(5**i) - half):
            m = 2
        elif base + 5**i + half >= num >= base + (5**i - half):
            m = 1
        elif base + half >= num >= base - half:
            m = 0
        elif base - (5**i - half) >= num >= base - (5**i + half):
            m = -1
        elif base - 2*(5**i) + half >= num >= base - 2*(5**i) - half:
            m = -2
        
        dec = dec + ('=' if m == -2 else '-' if m == -1 else str(m))
        base = base + m*(5**i)
    if dec[0] == str(0): dec = dec[1:len(dec)]
    return dec

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

sum = 0
for i in range(len(input)):
    sum += convert_snafu(input[i])

sum_snafu = convert_decimal(sum)

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(sum_snafu)))

########### NO PART TWO! ##################