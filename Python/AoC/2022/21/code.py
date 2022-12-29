# Advent of code Year 2022 Day 21 solution
# Author = Trent Huhn
# Date = December 2022

import time

def get_monkey(m,nums,opers):
    if m in nums.keys():
        return nums[m]

    m1 = get_monkey(opers[m][0],nums,opers)
    m2 = get_monkey(opers[m][1],nums,opers)
    nums[opers[m][0]] = m1
    nums[opers[m][1]] = m2
    if None in (m1,m2): # indicates that somewhere downstream was the human
        return None
    if opers[m][2] == '+':
        return m1 + m2
    elif opers[m][2] == '-':
        return m1 - m2
    elif opers[m][2] == '*':
        return m1 * m2
    elif opers[m][2] == '/':
        assert m2 != 0
        return m1 // m2 

def get_upstream(val,m,nums,opers):
    print('Examining {m} ({m1},{m2})'.format(m = m, m1 = opers[m][0], m2 = opers[m][1]))
    assert opers[m][0] in nums.keys() and opers[m][1] in nums.keys(), "All upstream monkeys should already be in the nums dict"
    oper = opers[m][2] # +,*,/
    assert nums[opers[m][0]] == None or nums[opers[m][1]] == None, "At least one upstream monkey should be un-assigned"
    missing = opers[m][0] if nums[opers[m][0]] == None else opers[m][1]

    # to unwind the stack, we perform slightly different actions depending on whether left or right value is missing
    if nums[opers[m][0]] == None: 
        assert nums[opers[m][1]] != None, "Only one monkey should be un-assigned"
        num = nums[opers[m][1]]
        if oper == '+':
            new_val = val - num
        elif oper == '-':
            new_val = val + num
        elif oper == '*':
            new_val = val // num
        elif oper == '/':
            new_val = val*num
        nums[opers[m][0]] = new_val
    elif nums[opers[m][1]] == None:
        assert nums[opers[m][0]] != None, "Only one monkey should be un-assigned"
        num = nums[opers[m][0]]
        if oper == '+':
            new_val = val - num
        elif oper == '-':
            new_val = num - val
        elif oper == '*':
            new_val = val // num
        elif oper == '/':
            new_val = num // val
        nums[opers[m][1]] = new_val

    if nums['humn'] != None: return
    get_upstream(new_val,missing,nums,opers)
    

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

nums = {} # dict to keep track of variables that we've resolved to a number
opers = {} # dict to keep track of pending operations
for i in range(len(input)):
    line = input[i].split(' ')
    var = ''.join(char for char in line[0] if char.isalpha()) # keep just alpha characters
    if line[1].isnumeric():
        nums[var] = int(line[1])
    else:
        assert line[1].isalpha() and line[3].isalpha()
        assert len(line[2]) == 1
        opers[var] = (line[1],line[3],line[2])

root = get_monkey('root',{key : val for (key,val) in nums.items()},opers) # create a new nums dict that will be mutated within the recursive function


part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(root)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

nums['humn'] = None # un-assign humn key
assert 'root' in opers.keys()

# get each of the two downstream values for our root monkey (the one containing the human should return None)
m1 = get_monkey(opers['root'][0],nums,opers) # this time we *do* want to mutate the nums dict
m2 = get_monkey(opers['root'][1],nums,opers)

monkey_val = m1 or m2 # this is the value we need to get to

get_upstream(monkey_val,opers['root'][0 if m2 else 1],nums,opers)
assert nums['humn'] != None, 'humn key not assigned, something went wrong'

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(nums['humn'])))