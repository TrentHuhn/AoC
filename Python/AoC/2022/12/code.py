# Advent of code Year 2022 Day 12 solution
# Author = Trent Huhn
# Date = December 2022

import time

# Python program for Dijkstra's single
# source shortest path algorithm. The program is
# for adjacency matrix representation of the graph
class Graph():
 
    def __init__(self, graph):
        self.graph = graph
 
    def printSolution(self, dist):
        print("Vertex \t Distance from Source")
        for i,key in enumerate(dist):
            print(key, "\t\t", dist[key])
 
    # Find the node with the minimum distance value, from the set of nodes not yet included in shortest path tree
    def minDistance(self, dist, sptSet):
 
        # Initialize minimum distance for next node
        min = 1e7
        min_index = None
        # Search next nearest node not in the
        # shortest path tree
        for i,key in enumerate(dist):
            if key in dist and dist[key] < min and sptSet[key] == False:
                min = dist[key]
                min_index = key
 
        return min_index
 
    # Implementation of Dijkstra's single source
    # shortest path algorithm for a graph represented
    # using adjacency matrix representation
    def dijkstra(self, src):
        
        # instantiate all distances as "infinity"
        dist = {key:1e7 for key, value in self.graph.items() if key in self.graph}
        dist[src] = 0
        sptSet = {key:False for key, value in self.graph.items() if key in self.graph}
 
        for i,key in enumerate(dist):
            # Pick the minimum distance node from
            # the set of nodes not yet processed.
            # u is always equal to src in first iteration
            u = self.minDistance(dist, sptSet)
            if u == None: break
 
            # Put the minimum distance node in the
            # shortest path tree
            sptSet[u] = True
 
            # Update dist value of the adjacent nodes
            # of the picked node only if the current
            # distance is greater than new distance and
            # the node in not in the shortest path tree
            for j,graph_key in enumerate(graph):
                if (graph_key in self.graph[u] and self.graph[u][graph_key] > 0 and
                   sptSet[graph_key] == False and
                   dist[graph_key] > dist[u] + self.graph[u][graph_key]):
                    dist[graph_key] = dist[u] + self.graph[u][graph_key]
 
        #self.printSolution(dist)
        return dist

def build_graph(heightmap):    
    x_size = len(heightmap[0])
    y_size = len(heightmap)
    
    graph = {} # instantiate new dictionary to use as our directed graph with the 2D coordinates as the key
    # loop through each node in height map to create our directed graph
    for i in range(y_size): 
        for j in range(x_size):
            graph[(i,j)] = {}
            cur_height = heightmap[i][j]        

            differenceX = [0,0,1,-1]
            differenceY = [-1,1,0,0]
            for dir in range(4): # update graph to show distances for up to four adjacent nodes
                neigh_row = i + differenceY[dir]
                neigh_col = j + differenceX[dir]
                if min(neigh_row, neigh_col) >= 0 and neigh_row < y_size and neigh_col < x_size and heightmap[neigh_row][neigh_col] <= cur_height + 1:
                    graph[(i,j)][(neigh_row,neigh_col)] = 1 # distance between adjacent nodes that meet our criteria will always be 1
    
    return graph

with open((__file__.rstrip("code.py")+"input.txt"), 'r') as input_file:
    input = input_file.read().split("\n")

SKIP_PART_ONE = False

########### PART ONE ##################
print("Starting Part One")
start_time = time.time()

heightmap = [[ord(x) - 97 for x in line] for line in input]

# find starting and ending positions
start_pos = [(i,j) for i,row in enumerate(heightmap) for j,col in enumerate(row) if col == -14][0] # -14 is offset for S
end_pos = [(i,j) for i,row in enumerate(heightmap) for j,col in enumerate(row) if col == -28][0] # -28 is offset for E
# replace start/end positions in heightmap
heightmap[start_pos[0]][start_pos[1]] = 0 # start is 'a' (0)
heightmap[end_pos[0]][end_pos[1]] = 25 # end is 'z' (25)

graph = build_graph(heightmap)
g = Graph(graph) 

if not SKIP_PART_ONE:
    distances = g.dijkstra(start_pos)

    part_one_time = time.time() - start_time
    print("Part One ({time} s): {value}".format(time = round(part_one_time,3), value = str(distances[end_pos])))

########### PART TWO ##################
print("Starting Part Two")
start_time = time.time()

# invert our heightmap and re-build the directed graph
heightmap = [[-1*heightmap[i][j] for j in range(len(heightmap[0]))] for i in range(len(heightmap))]

# find all possible starting positions
possible_starts = [(i,j) for i,row in enumerate(heightmap) for j,col in enumerate(row) if col == 0]

min_steps = 1e7
start_pos = None

graph = build_graph(heightmap)
g = Graph(graph)
distances = g.dijkstra(end_pos) # start at the end pos (guarantees we will travel to every possible node)
for i in range(len(possible_starts)):
    # check to see which of the start positions have the smallest # of steps going back from the end
    if distances[possible_starts[i]] < min_steps:
        min_steps = distances[possible_starts[i]]
        start_pos = possible_starts[i]



part_two_time = time.time() - start_time
print("Part Two ({time} s): {value} (start position: {start_pos})".format(time = round(part_two_time,3), value = str(min_steps), start_pos = str(start_pos)))