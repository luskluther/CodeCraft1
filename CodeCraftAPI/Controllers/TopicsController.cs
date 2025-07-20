using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeCraftAPI.Data;

namespace CodeCraftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public TopicsController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTopics()
        {
            var topics = await _context.Topics
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.Category,
                    t.Icon,
                    t.Intro,
                    t.Description,
                    t.Code,
                    t.DiagramData
                })
                .ToListAsync();
                
            return Ok(new { success = true, data = topics });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopic(int id)
        {
            var topic = await _context.Topics
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.Category,
                    t.Icon,
                    t.Intro,
                    t.Description,
                    t.Code,
                    t.DiagramData
                })
                .FirstOrDefaultAsync();
                
            if (topic == null)
                return NotFound(new { success = false, message = "Topic not found" });
                
            return Ok(new { success = true, data = topic });
        }
        
        [HttpPost("update-diagrams")]
        public async Task<IActionResult> UpdateDiagrams()
        {
            var diagramData = new Dictionary<string, string>
            {
                ["Trees"] = @"<svg viewBox=""0 0 400 300"" class=""diagram-svg"">
            <circle cx=""200"" cy=""50"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""200"" y=""57"" text-anchor=""middle"" font-size=""16"" fill=""#333"">A</text>
            
            <circle cx=""120"" cy=""150"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""120"" y=""157"" text-anchor=""middle"" font-size=""16"" fill=""#333"">B</text>
            
            <circle cx=""280"" cy=""150"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""280"" y=""157"" text-anchor=""middle"" font-size=""16"" fill=""#333"">C</text>
            
            <circle cx=""80"" cy=""250"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""80"" y=""257"" text-anchor=""middle"" font-size=""16"" fill=""#333"">D</text>
            
            <circle cx=""160"" cy=""250"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""160"" y=""257"" text-anchor=""middle"" font-size=""16"" fill=""#333"">E</text>
            
            <line x1=""200"" y1=""80"" x2=""120"" y2=""120"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""200"" y1=""80"" x2=""280"" y2=""120"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""120"" y1=""180"" x2=""80"" y2=""220"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""120"" y1=""180"" x2=""160"" y2=""220"" stroke=""#666"" stroke-width=""2""/>
        </svg>",
        
                ["Linked Lists"] = @"<svg viewBox=""0 0 500 200"" class=""diagram-svg"">
            <rect x=""50"" y=""70"" width=""80"" height=""60"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2"" rx=""5""/>
            <text x=""90"" y=""95"" text-anchor=""middle"" font-size=""14"" fill=""#333"">Data: 5</text>
            <rect x=""70"" y=""100"" width=""40"" height=""20"" fill=""#fff"" stroke=""#666"" stroke-width=""1""/>
            <text x=""90"" y=""115"" text-anchor=""middle"" font-size=""12"" fill=""#333"">next</text>
            
            <rect x=""170"" y=""70"" width=""80"" height=""60"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2"" rx=""5""/>
            <text x=""210"" y=""95"" text-anchor=""middle"" font-size=""14"" fill=""#333"">Data: 10</text>
            <rect x=""190"" y=""100"" width=""40"" height=""20"" fill=""#fff"" stroke=""#666"" stroke-width=""1""/>
            <text x=""210"" y=""115"" text-anchor=""middle"" font-size=""12"" fill=""#333"">next</text>
            
            <rect x=""290"" y=""70"" width=""80"" height=""60"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2"" rx=""5""/>
            <text x=""330"" y=""95"" text-anchor=""middle"" font-size=""14"" fill=""#333"">Data: 15</text>
            <rect x=""310"" y=""100"" width=""40"" height=""20"" fill=""#fff"" stroke=""#666"" stroke-width=""1""/>
            <text x=""330"" y=""115"" text-anchor=""middle"" font-size=""12"" fill=""#333"">null</text>
            
            <path d=""M 130 100 L 165 100"" stroke=""#666"" stroke-width=""2"" marker-end=""url(#arrowhead)""/>
            <path d=""M 250 100 L 285 100"" stroke=""#666"" stroke-width=""2"" marker-end=""url(#arrowhead)""/>
            
            <defs>
                <marker id=""arrowhead"" markerWidth=""10"" markerHeight=""7"" refX=""9"" refY=""3.5"" orient=""auto"">
                    <polygon points=""0 0, 10 3.5, 0 7"" fill=""#666""/>
                </marker>
            </defs>
        </svg>",
        
                ["Hash Tables"] = @"<svg viewBox=""0 0 400 300"" class=""diagram-svg"">
            <text x=""50"" y=""30"" font-size=""14"" fill=""#333"">Key → Hash → Index</text>
            
            <rect x=""50"" y=""60"" width=""60"" height=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""80"" y=""80"" text-anchor=""middle"" font-size=""12"" fill=""#333"">""cat""</text>
            
            <rect x=""50"" y=""100"" width=""60"" height=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""80"" y=""120"" text-anchor=""middle"" font-size=""12"" fill=""#333"">""dog""</text>
            
            <rect x=""50"" y=""140"" width=""60"" height=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""80"" y=""160"" text-anchor=""middle"" font-size=""12"" fill=""#333"">""fox""</text>
            
            <text x=""150"" y=""115"" text-anchor=""middle"" font-size=""24"" fill=""#666"">→</text>
            
            <rect x=""200"" y=""50"" width=""150"" height=""200"" fill=""#f0f0f0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""275"" y=""40"" text-anchor=""middle"" font-size=""14"" fill=""#333"">Hash Table</text>
            
            <rect x=""210"" y=""60"" width=""130"" height=""25"" fill=""#fff"" stroke=""#666"" stroke-width=""1""/>
            <text x=""215"" y=""77"" font-size=""12"" fill=""#333"">0: </text>
            
            <rect x=""210"" y=""90"" width=""130"" height=""25"" fill=""#fff"" stroke=""#666"" stroke-width=""1""/>
            <text x=""215"" y=""107"" font-size=""12"" fill=""#333"">1: ""dog"" → 🐕</text>
            
            <rect x=""210"" y=""120"" width=""130"" height=""25"" fill=""#fff"" stroke=""#666"" stroke-width=""1""/>
            <text x=""215"" y=""137"" font-size=""12"" fill=""#333"">2: ""cat"" → 🐱</text>
            
            <rect x=""210"" y=""150"" width=""130"" height=""25"" fill=""#fff"" stroke=""#666"" stroke-width=""1""/>
            <text x=""215"" y=""167"" font-size=""12"" fill=""#333"">3: </text>
            
            <rect x=""210"" y=""180"" width=""130"" height=""25"" fill=""#fff"" stroke=""#666"" stroke-width=""1""/>
            <text x=""215"" y=""197"" font-size=""12"" fill=""#333"">4: ""fox"" → 🦊</text>
        </svg>",
        
                ["Graphs"] = @"<svg viewBox=""0 0 400 300"" class=""diagram-svg"">
            <circle cx=""200"" cy=""50"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""200"" y=""57"" text-anchor=""middle"" font-size=""16"" fill=""#333"">A</text>
            
            <circle cx=""100"" cy=""150"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""100"" y=""157"" text-anchor=""middle"" font-size=""16"" fill=""#333"">B</text>
            
            <circle cx=""300"" cy=""150"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""300"" y=""157"" text-anchor=""middle"" font-size=""16"" fill=""#333"">C</text>
            
            <circle cx=""150"" cy=""250"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""150"" y=""257"" text-anchor=""middle"" font-size=""16"" fill=""#333"">D</text>
            
            <circle cx=""250"" cy=""250"" r=""30"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""250"" y=""257"" text-anchor=""middle"" font-size=""16"" fill=""#333"">E</text>
            
            <line x1=""200"" y1=""80"" x2=""100"" y2=""120"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""200"" y1=""80"" x2=""300"" y2=""120"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""100"" y1=""180"" x2=""150"" y2=""220"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""300"" y1=""180"" x2=""250"" y2=""220"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""150"" y1=""250"" x2=""250"" y2=""250"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""100"" y1=""150"" x2=""300"" y2=""150"" stroke=""#666"" stroke-width=""2""/>
        </svg>",
        
                ["Sorting"] = @"<svg viewBox=""0 0 500 250"" class=""diagram-svg"">
            <text x=""250"" y=""30"" text-anchor=""middle"" font-size=""16"" fill=""#333"">Quick Sort Example</text>
            
            <g transform=""translate(50, 60)"">
                <rect x=""0"" y=""0"" width=""40"" height=""40"" fill=""#ffcdd2"" stroke=""#666"" stroke-width=""2""/>
                <text x=""20"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">8</text>
                
                <rect x=""50"" y=""0"" width=""40"" height=""40"" fill=""#ffcdd2"" stroke=""#666"" stroke-width=""2""/>
                <text x=""70"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">3</text>
                
                <rect x=""100"" y=""0"" width=""40"" height=""40"" fill=""#c5e1a5"" stroke=""#666"" stroke-width=""2""/>
                <text x=""120"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">5</text>
                
                <rect x=""150"" y=""0"" width=""40"" height=""40"" fill=""#ffcdd2"" stroke=""#666"" stroke-width=""2""/>
                <text x=""170"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">1</text>
                
                <rect x=""200"" y=""0"" width=""40"" height=""40"" fill=""#ffcdd2"" stroke=""#666"" stroke-width=""2""/>
                <text x=""220"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">9</text>
                
                <text x=""120"" y=""-10"" text-anchor=""middle"" font-size=""12"" fill=""#666"">pivot</text>
            </g>
            
            <text x=""250"" y=""140"" text-anchor=""middle"" font-size=""14"" fill=""#666"">↓</text>
            
            <g transform=""translate(50, 160)"">
                <rect x=""0"" y=""0"" width=""40"" height=""40"" fill=""#bbdefb"" stroke=""#666"" stroke-width=""2""/>
                <text x=""20"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">1</text>
                
                <rect x=""50"" y=""0"" width=""40"" height=""40"" fill=""#bbdefb"" stroke=""#666"" stroke-width=""2""/>
                <text x=""70"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">3</text>
                
                <rect x=""100"" y=""0"" width=""40"" height=""40"" fill=""#c5e1a5"" stroke=""#666"" stroke-width=""2""/>
                <text x=""120"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">5</text>
                
                <rect x=""150"" y=""0"" width=""40"" height=""40"" fill=""#bbdefb"" stroke=""#666"" stroke-width=""2""/>
                <text x=""170"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">8</text>
                
                <rect x=""200"" y=""0"" width=""40"" height=""40"" fill=""#bbdefb"" stroke=""#666"" stroke-width=""2""/>
                <text x=""220"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">9</text>
                
                <text x=""120"" y=""-10"" text-anchor=""middle"" font-size=""12"" fill=""#666"">sorted</text>
            </g>
        </svg>",
        
                ["Searching"] = @"<svg viewBox=""0 0 500 200"" class=""diagram-svg"">
            <text x=""250"" y=""30"" text-anchor=""middle"" font-size=""16"" fill=""#333"">Binary Search for 7</text>
            
            <g transform=""translate(50, 60)"">
                <rect x=""0"" y=""0"" width=""40"" height=""40"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
                <text x=""20"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">1</text>
                
                <rect x=""50"" y=""0"" width=""40"" height=""40"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
                <text x=""70"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">3</text>
                
                <rect x=""100"" y=""0"" width=""40"" height=""40"" fill=""#ffeb3b"" stroke=""#666"" stroke-width=""2""/>
                <text x=""120"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">5</text>
                
                <rect x=""150"" y=""0"" width=""40"" height=""40"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
                <text x=""170"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">7</text>
                
                <rect x=""200"" y=""0"" width=""40"" height=""40"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
                <text x=""220"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">9</text>
                
                <text x=""120"" y=""-10"" text-anchor=""middle"" font-size=""12"" fill=""#666"">mid</text>
            </g>
            
            <text x=""250"" y=""130"" text-anchor=""middle"" font-size=""14"" fill=""#666"">7 > 5, search right half</text>
            
            <g transform=""translate(175, 150)"">
                <rect x=""0"" y=""0"" width=""40"" height=""40"" fill=""#c5e1a5"" stroke=""#666"" stroke-width=""2""/>
                <text x=""20"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">7</text>
                
                <rect x=""50"" y=""0"" width=""40"" height=""40"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
                <text x=""70"" y=""25"" text-anchor=""middle"" font-size=""14"" fill=""#333"">9</text>
                
                <text x=""20"" y=""-10"" text-anchor=""middle"" font-size=""12"" fill=""#666"">found!</text>
            </g>
        </svg>",
        
                ["Traversal"] = @"<svg viewBox=""0 0 400 300"" class=""diagram-svg"">
            <text x=""200"" y=""30"" text-anchor=""middle"" font-size=""16"" fill=""#333"">Tree Traversal Order</text>
            
            <circle cx=""200"" cy=""80"" r=""25"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""200"" y=""87"" text-anchor=""middle"" font-size=""14"" fill=""#333"">A</text>
            
            <circle cx=""130"" cy=""160"" r=""25"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""130"" y=""167"" text-anchor=""middle"" font-size=""14"" fill=""#333"">B</text>
            
            <circle cx=""270"" cy=""160"" r=""25"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""270"" y=""167"" text-anchor=""middle"" font-size=""14"" fill=""#333"">C</text>
            
            <circle cx=""90"" cy=""240"" r=""25"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""90"" y=""247"" text-anchor=""middle"" font-size=""14"" fill=""#333"">D</text>
            
            <circle cx=""170"" cy=""240"" r=""25"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""170"" y=""247"" text-anchor=""middle"" font-size=""14"" fill=""#333"">E</text>
            
            <line x1=""200"" y1=""105"" x2=""130"" y2=""135"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""200"" y1=""105"" x2=""270"" y2=""135"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""130"" y1=""185"" x2=""90"" y2=""215"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""130"" y1=""185"" x2=""170"" y2=""215"" stroke=""#666"" stroke-width=""2""/>
            
            <text x=""50"" y=""280"" font-size=""12"" fill=""#333"">Inorder: D→B→E→A→C</text>
            <text x=""200"" y=""280"" font-size=""12"" fill=""#333"">Preorder: A→B→D→E→C</text>
        </svg>",
        
                ["Dynamic Programming"] = @"<svg viewBox=""0 0 500 300"" class=""diagram-svg"">
            <text x=""250"" y=""30"" text-anchor=""middle"" font-size=""16"" fill=""#333"">Fibonacci with Memoization</text>
            
            <rect x=""200"" y=""60"" width=""100"" height=""40"" fill=""#c5e1a5"" stroke=""#666"" stroke-width=""2""/>
            <text x=""250"" y=""85"" text-anchor=""middle"" font-size=""14"" fill=""#333"">fib(5)</text>
            
            <rect x=""100"" y=""130"" width=""100"" height=""40"" fill=""#bbdefb"" stroke=""#666"" stroke-width=""2""/>
            <text x=""150"" y=""155"" text-anchor=""middle"" font-size=""14"" fill=""#333"">fib(4)</text>
            
            <rect x=""300"" y=""130"" width=""100"" height=""40"" fill=""#bbdefb"" stroke=""#666"" stroke-width=""2""/>
            <text x=""350"" y=""155"" text-anchor=""middle"" font-size=""14"" fill=""#333"">fib(3)</text>
            
            <rect x=""50"" y=""200"" width=""80"" height=""35"" fill=""#ffcdd2"" stroke=""#666"" stroke-width=""2""/>
            <text x=""90"" y=""222"" text-anchor=""middle"" font-size=""12"" fill=""#333"">fib(3)</text>
            
            <rect x=""150"" y=""200"" width=""80"" height=""35"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""190"" y=""222"" text-anchor=""middle"" font-size=""12"" fill=""#333"">fib(2)=1</text>
            
            <rect x=""270"" y=""200"" width=""80"" height=""35"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""310"" y=""222"" text-anchor=""middle"" font-size=""12"" fill=""#333"">fib(2)=1</text>
            
            <rect x=""370"" y=""200"" width=""80"" height=""35"" fill=""#e0e0e0"" stroke=""#666"" stroke-width=""2""/>
            <text x=""410"" y=""222"" text-anchor=""middle"" font-size=""12"" fill=""#333"">fib(1)=1</text>
            
            <line x1=""250"" y1=""100"" x2=""150"" y2=""130"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""250"" y1=""100"" x2=""350"" y2=""130"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""150"" y1=""170"" x2=""90"" y2=""200"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""150"" y1=""170"" x2=""190"" y2=""200"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""350"" y1=""170"" x2=""310"" y2=""200"" stroke=""#666"" stroke-width=""2""/>
            <line x1=""350"" y1=""170"" x2=""410"" y2=""200"" stroke=""#666"" stroke-width=""2""/>
            
            <text x=""250"" y=""270"" text-anchor=""middle"" font-size=""12"" fill=""#666"">Cached results prevent recalculation</text>
        </svg>"
            };
            
            int updatedCount = 0;
            
            foreach (var (topicName, diagram) in diagramData)
            {
                var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Name == topicName);
                if (topic != null)
                {
                    topic.DiagramData = diagram;
                    updatedCount++;
                }
            }
            
            await _context.SaveChangesAsync();
            
            return Ok(new { success = true, message = $"Updated {updatedCount} topics with diagram data" });
        }
    }
}