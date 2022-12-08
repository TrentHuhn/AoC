# Advent of code Year 2022 Day 7 solution
# Author = Trent Huhn
# Date = December 2022

import time

class File:
    def __init__(self, name, size) -> None:
        self.name = name
        self.size = size

class Directory:
    def __init__(self, name) -> None:
        self.name = name
        self.size = 0
        self.files = []
        self.directories = {}

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")


########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

curCommand = ''
curDirPath = []
fileStruct = {'/': 0} # instantiate root directory
for i,line in enumerate(input):
    line = line.split(' ')
    if line[0] == '$': 
        if line[1] == "cd":
            print("Change directory to {dir}".format(dir = line[2]))
            # need to handle finding current directory
            curDir = line[2]
            if curDir == '..': # go up one level 
                curDir = curDirPath.pop()
            else: 
                if curDir == '/':
                    curDirPath = []
                curDirPath.append(curDir)
                
        elif line[1] == "ls": # assume we're parsing an ls output
            print("Parsing ls command")
    else: # parsing ls output
        if line[0] == "dir":
            print("Parsing directory {dir}".format(dir = line[1]))
            fileStruct[''.join(curDirPath) + line[1]] = 0 #instantiate new directory with size 0
        else:
            print("Parsing file {file} with size {size}".format(file = line[1], size = line[0]))
            fileStruct[''.join(curDirPath)] += int(line[0]) # if this is a file, increment size of current directory
            for i in range(1,len(curDirPath)):
                fileStruct[''.join(curDirPath[:-i])] += int(line[0])

# now find all directories < 100000 in size
totalSize = 0
for i,dir in enumerate(fileStruct):
    size = fileStruct[dir]
    if size <= 100000: totalSize += size

part_one_time = time.time() - start_time
print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(totalSize)))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

totalSpace = 70000000
spaceNeeded = 30000000
usedSpace = fileStruct['/']
unusedSpace = totalSpace - usedSpace
print("Current used space: {usedSpace}, unused space: {unused}".format(usedSpace =  usedSpace, unused = unusedSpace))
sizeToDelete = spaceNeeded - unusedSpace
winner = totalSpace
if unusedSpace < spaceNeeded:
    for i,dir in enumerate(fileStruct):
        size = fileStruct[dir]
        if size >= sizeToDelete and size < winner: winner = size
else:
    print("We already have enough space for the update!")

part_two_time = time.time() - start_time
print("Part Two ({time} s): {value}".format(time = round(part_two_time,3), value = str(winner)))