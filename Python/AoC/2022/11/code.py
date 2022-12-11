# Advent of code Year 2022 Day 11 solution
# Author = Trent Huhn
# Date = December 2022

import math
import time

class Monkey:
    def __init__(self) -> None:
        self.items = []
        self.opr_mult = 1
        self.opr_add = 0
        self.opr_square = False
        self.test_val = None
        self.true_result = None
        self.false_result = None
        self.items_inspected = 0

def build_monkeys(input):
    monkeys = []
    for i in range(0,len(input)):
        if not str.lower(input[i].split(' ')[0]) == "monkey":
            continue

        monkey = Monkey()
        # get list of items
        monkey.items = list(map(int, input[i+1].split(':')[-1].split(', ')))
        
        # get info on operation
        opr_str = input[i+2].split(' ')
        if opr_str[-2] == '*':
            if opr_str[-1] != 'old': monkey.opr_mult = int(opr_str[-1])
            else: monkey.opr_square = True
        elif opr_str[-2] == '+': monkey.opr_add = int(opr_str[-1])
        else:
            print("Unrecognized operator symbol")
            continue

        # get info on test
        monkey.test_val = int(input[i+3].split(' ')[-1])

        # get info on test results
        monkey.true_result = int(input[i+4].split(' ')[-1])
        monkey.false_result = int(input[i+5].split(' ')[-1])

        monkeys.append(monkey)
    
    return monkeys



with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

NUM_ROUNDS = 20

# build initial monkey states
monkeys_pt1 = build_monkeys(input)

# Start tracking rounds!
for i in range(0,NUM_ROUNDS):
    for j,monkey in enumerate(monkeys_pt1):
        # inspect each item
        while(monkey.items):
            item = monkey.items.pop(0) # grab the first item in the list
            item = item*monkey.opr_mult + monkey.opr_add if not monkey.opr_square else item*item # step one - increase worry level
            item = item // 3 # step two - decrease worry level (use integer division)
            if(item % monkey.test_val == 0):
                monkeys_pt1[monkey.true_result].items.append(item)
            else:
                monkeys_pt1[monkey.false_result].items.append(item)
            monkey.items_inspected += 1

sizes = [m.items_inspected for m in monkeys_pt1]
sizes.sort()

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(sizes[-1]*sizes[-2])))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

NUM_ROUNDS = 10000

# build initial monkey states
monkeys_pt2 = build_monkeys(input)

# For part 2, we will only store the result of each item mod product(all divisor tests)
# E.g. if the current worry level is 101 and divisor tests are 13, 3, 7, 2, then each time we will store: 101  % 576

product = math.prod([m.test_val for m in monkeys_pt2])

# Start tracking rounds!
for i in range(0,NUM_ROUNDS):
    for j,monkey in enumerate(monkeys_pt2):
        # inspect each item
        while(monkey.items):
            item = monkey.items.pop(0) # grab the first item in the list
            item = item*monkey.opr_mult + monkey.opr_add if not monkey.opr_square else item*item # step one - increase worry level
            # no longer decrease worry level each time an item is inspected
            if(item % monkey.test_val == 0):
                monkeys_pt2[monkey.true_result].items.append(item % product)
            else:
                monkeys_pt2[monkey.false_result].items.append(item % product)
            monkey.items_inspected += 1

sizes = [m.items_inspected for m in monkeys_pt2]
sizes.sort()


part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(sizes[-1]*sizes[-2])))