# Advent of code Year 2022 Day 10 solution
# Author = Trent Huhn
# Date = December 2022

import time

def evaluate_cycle(cycle,x_val,strengths):    
    if cycle in EVALUATE_CYCLES:
        strengths.append(cycle*x_val) # add signal strength to our output list
    cycle += 1
    return cycle,strengths

def evaluate_cycle_pt2(cycle,x_val,output):
    pixel = '.'   
    if len(output) <= (cycle - 1) // HORZ_PIXELS: output.append([]) # use integer division to check if we have to add a new row to our output array
    if (cycle - 1) % HORZ_PIXELS in [x_val-1,x_val,x_val+1]: # use modulo to determine current horizontal position based on cycle #
        pixel = '#'
    output[(cycle - 1) // HORZ_PIXELS].append(pixel)
    cycle += 1
    return cycle,output

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

EVALUATE_CYCLES = [20,60,100,140,180,220]
x_val = 1
cycle = 1
signal_strengths = []
val = 0

for i,line in enumerate(input):
    line = line.split(' ')
    inst = line[0]
    if len(line) > 1: val = int(line[1])

    # increment cycle and check to see if this cycle needs to have its signal strength calculated
    cycle,signal_strengths = evaluate_cycle(cycle,x_val,signal_strengths) 

    if inst == "addx":
        cycle,signal_strengths = evaluate_cycle(cycle,x_val,signal_strengths) 
        x_val += val # finally, finish the addx instruction

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(sum(signal_strengths))))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

x_val = 1
cycle = 1
HORZ_PIXELS = 40
output = []
val = 0
for i,line in enumerate(input):
    line = line.split(' ')
    inst = line[0]
    if len(line) > 1: val = int(line[1])

    # increment cycle and update output array based on current sprite position
    cycle,output = evaluate_cycle_pt2(cycle,x_val,output) 

    if inst == "addx":
        cycle,output = evaluate_cycle_pt2(cycle,x_val,output) 
        x_val += val # finally, finish the addx instruction

# Print the output to the screen
for i in range (0,len(output)):
    print(''.join(output[i]))


part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(None)))