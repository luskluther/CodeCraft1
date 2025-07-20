using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodeCraftAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    Intro = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Avatar = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProgress",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgress", x => new { x.UserId, x.TopicId });
                    table.ForeignKey(
                        name: "FK_UserProgress_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProgress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Category", "Code", "Description", "Icon", "Intro", "Name" },
                values: new object[,]
                {
                    { 1, "dataStructures", "class TreeNode:\r\n    def __init__(self, data):\r\n        self.data = data\r\n        self.children = []\r\n    \r\n    def add_child(self, child):\r\n        self.children.append(child)", "Trees are hierarchical data structures consisting of nodes connected by edges. Each tree has a root node at the top, and every other node has exactly one parent.", "🌳", "Hierarchical data structures with nodes connected by edges.", "Trees" },
                    { 2, "dataStructures", "class Node:\r\n    def __init__(self, data):\r\n        self.data = data\r\n        self.next = None\r\n\r\nclass LinkedList:\r\n    def __init__(self):\r\n        self.head = None", "Linked lists are a linear data structure where elements are not stored in contiguous memory locations.", "🔗", "Linear collection of elements stored in nodes.", "Linked Lists" },
                    { 3, "dataStructures", "class HashTable:\r\n    def __init__(self, size=10):\r\n        self.size = size\r\n        self.table = [[] for _ in range(size)]", "Hash tables use a hash function to compute an index into an array of buckets.", "#️⃣", "Key-value pairs for fast lookup.", "Hash Tables" },
                    { 4, "dataStructures", "class Graph:\r\n    def __init__(self):\r\n        self.adjacency_list = {}", "Graphs are non-linear data structures consisting of vertices and edges.", "🕸️", "Networks of nodes and edges.", "Graphs" },
                    { 5, "algorithms", "def quick_sort(arr):\r\n    if len(arr) <= 1:\r\n        return arr\r\n    pivot = arr[len(arr) // 2]\r\n    return quick_sort([x for x in arr if x < pivot]) + [pivot] + quick_sort([x for x in arr if x > pivot])", "Sorting algorithms rearrange elements of a list or array in a certain order.", "📊", "Algorithms for ordering elements.", "Sorting" },
                    { 6, "algorithms", "def binary_search(arr, target):\r\n    left, right = 0, len(arr) - 1\r\n    while left <= right:\r\n        mid = (left + right) // 2\r\n        if arr[mid] == target:\r\n            return mid\r\n        elif arr[mid] < target:\r\n            left = mid + 1\r\n        else:\r\n            right = mid - 1\r\n    return -1", "Searching algorithms are used to find specific elements in a collection.", "🔍", "Algorithms for finding elements.", "Searching" },
                    { 7, "algorithms", "def inorder_traversal(root):\r\n    if root:\r\n        inorder_traversal(root.left)\r\n        print(root.data)\r\n        inorder_traversal(root.right)", "Traversal algorithms visit each node in data structures.", "🚶", "Algorithms for visiting nodes systematically.", "Traversal" },
                    { 8, "algorithms", "def fibonacci(n, memo={}):\r\n    if n in memo:\r\n        return memo[n]\r\n    if n <= 2:\r\n        return 1\r\n    memo[n] = fibonacci(n-1, memo) + fibonacci(n-2, memo)\r\n    return memo[n]", "Dynamic programming solves complex problems by breaking them into simpler subproblems.", "⚙️", "Solving problems by breaking into subproblems.", "Dynamic Programming" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProgress_TopicId",
                table: "UserProgress",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProgress");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
