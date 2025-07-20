using Microsoft.EntityFrameworkCore;
using CodeCraftAPI.Models.Entities;

namespace CodeCraftAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key for UserProgress
            modelBuilder.Entity<UserProgress>()
                .HasKey(up => new { up.UserId, up.TopicId });
                
            // Relationships
            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.User)
                .WithMany(u => u.Progress)
                .HasForeignKey(up => up.UserId);
                
            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.Topic)
                .WithMany(t => t.UserProgress)
                .HasForeignKey(up => up.TopicId);
                
            // Seed topics
            SeedTopics(modelBuilder);
        }
        
        private void SeedTopics(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>().HasData(
                new Topic 
                { 
                    Id = 1, 
                    Name = "Trees", 
                    Category = "dataStructures", 
                    Icon = "🌳",
                    Intro = "Hierarchical data structures with nodes connected by edges.",
                    Description = "Trees are hierarchical data structures consisting of nodes connected by edges. Each tree has a root node at the top, and every other node has exactly one parent.",
                    Code = @"class TreeNode:
    def __init__(self, data):
        self.data = data
        self.children = []
    
    def add_child(self, child):
        self.children.append(child)"
                },
                new Topic 
                { 
                    Id = 2, 
                    Name = "Linked Lists", 
                    Category = "dataStructures", 
                    Icon = "🔗",
                    Intro = "Linear collection of elements stored in nodes.",
                    Description = "Linked lists are a linear data structure where elements are not stored in contiguous memory locations.",
                    Code = @"class Node:
    def __init__(self, data):
        self.data = data
        self.next = None

class LinkedList:
    def __init__(self):
        self.head = None"
                },
                new Topic 
                { 
                    Id = 3, 
                    Name = "Hash Tables", 
                    Category = "dataStructures", 
                    Icon = "#️⃣",
                    Intro = "Key-value pairs for fast lookup.",
                    Description = "Hash tables use a hash function to compute an index into an array of buckets.",
                    Code = @"class HashTable:
    def __init__(self, size=10):
        self.size = size
        self.table = [[] for _ in range(size)]"
                },
                new Topic 
                { 
                    Id = 4, 
                    Name = "Graphs", 
                    Category = "dataStructures", 
                    Icon = "🕸️",
                    Intro = "Networks of nodes and edges.",
                    Description = "Graphs are non-linear data structures consisting of vertices and edges.",
                    Code = @"class Graph:
    def __init__(self):
        self.adjacency_list = {}"
                },
                new Topic 
                { 
                    Id = 5, 
                    Name = "Sorting", 
                    Category = "algorithms", 
                    Icon = "📊",
                    Intro = "Algorithms for ordering elements.",
                    Description = "Sorting algorithms rearrange elements of a list or array in a certain order.",
                    Code = @"def quick_sort(arr):
    if len(arr) <= 1:
        return arr
    pivot = arr[len(arr) // 2]
    return quick_sort([x for x in arr if x < pivot]) + [pivot] + quick_sort([x for x in arr if x > pivot])"
                },
                new Topic 
                { 
                    Id = 6, 
                    Name = "Searching", 
                    Category = "algorithms", 
                    Icon = "🔍",
                    Intro = "Algorithms for finding elements.",
                    Description = "Searching algorithms are used to find specific elements in a collection.",
                    Code = @"def binary_search(arr, target):
    left, right = 0, len(arr) - 1
    while left <= right:
        mid = (left + right) // 2
        if arr[mid] == target:
            return mid
        elif arr[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    return -1"
                },
                new Topic 
                { 
                    Id = 7, 
                    Name = "Traversal", 
                    Category = "algorithms", 
                    Icon = "🚶",
                    Intro = "Algorithms for visiting nodes systematically.",
                    Description = "Traversal algorithms visit each node in data structures.",
                    Code = @"def inorder_traversal(root):
    if root:
        inorder_traversal(root.left)
        print(root.data)
        inorder_traversal(root.right)"
                },
                new Topic 
                { 
                    Id = 8, 
                    Name = "Dynamic Programming", 
                    Category = "algorithms", 
                    Icon = "⚙️",
                    Intro = "Solving problems by breaking into subproblems.",
                    Description = "Dynamic programming solves complex problems by breaking them into simpler subproblems.",
                    Code = @"def fibonacci(n, memo={}):
    if n in memo:
        return memo[n]
    if n <= 2:
        return 1
    memo[n] = fibonacci(n-1, memo) + fibonacci(n-2, memo)
    return memo[n]"
                }
            );
        }
    }
}