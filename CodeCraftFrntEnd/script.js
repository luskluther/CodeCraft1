// Configuration
  const API_BASE_URL = window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1'
      ? 'http://localhost:5115/api'
      : 'http://vijaysfirstapp.duckdns.org:5115/api';
const USE_MOCK_API = false; // Set to false when backend is ready

// State
let currentUser = null;
let authToken = null;
let userProgress = {};

// Topic data
const topicData = {
    'Trees': {
        icon: '🌳',
        category: 'dataStructures',
        intro: 'Hierarchical data structures with nodes connected by edges.',
        description: 'Trees are hierarchical data structures consisting of nodes connected by edges. Each tree has a root node at the top, and every other node has exactly one parent.',
        code: `class TreeNode:
    def __init__(self, data):
        self.data = data
        self.children = []
    
    def add_child(self, child):
        self.children.append(child)`
    },
    'Linked Lists': {
        icon: '🔗',
        category: 'dataStructures',
        intro: 'Linear collection of elements stored in nodes.',
        description: 'Linked lists are a linear data structure where elements are not stored in contiguous memory locations.',
        code: `class Node:
    def __init__(self, data):
        self.data = data
        self.next = None

class LinkedList:
    def __init__(self):
        self.head = None`
    },
    'Hash Tables': {
        icon: '#️⃣',
        category: 'dataStructures',
        intro: 'Key-value pairs for fast lookup.',
        description: 'Hash tables use a hash function to compute an index into an array of buckets.',
        code: `class HashTable:
    def __init__(self, size=10):
        self.size = size
        self.table = [[] for _ in range(size)]`
    },
    'Graphs': {
        icon: '🕸️',
        category: 'dataStructures',
        intro: 'Networks of nodes and edges.',
        description: 'Graphs are non-linear data structures consisting of vertices and edges.',
        code: `class Graph:
    def __init__(self):
        self.adjacency_list = {}`
    },
    'Sorting': {
        icon: '📊',
        category: 'algorithms',
        intro: 'Algorithms for ordering elements.',
        description: 'Sorting algorithms rearrange elements of a list or array in a certain order.',
        code: `def quick_sort(arr):
    if len(arr) <= 1:
        return arr
    pivot = arr[len(arr) // 2]
    return quick_sort([x for x in arr if x < pivot]) + [pivot] + quick_sort([x for x in arr if x > pivot])`
    },
    'Searching': {
        icon: '🔍',
        category: 'algorithms',
        intro: 'Algorithms for finding elements.',
        description: 'Searching algorithms are used to find specific elements in a collection.',
        code: `def binary_search(arr, target):
    left, right = 0, len(arr) - 1
    while left <= right:
        mid = (left + right) // 2
        if arr[mid] == target:
            return mid
        elif arr[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    return -1`
    },
    'Traversal': {
        icon: '🚶',
        category: 'algorithms',
        intro: 'Algorithms for visiting nodes systematically.',
        description: 'Traversal algorithms visit each node in data structures.',
        code: `def inorder_traversal(root):
    if root:
        inorder_traversal(root.left)
        print(root.data)
        inorder_traversal(root.right)`
    },
    'Dynamic Programming': {
        icon: '⚙️',
        category: 'algorithms',
        intro: 'Solving problems by breaking into subproblems.',
        description: 'Dynamic programming solves complex problems by breaking them into simpler subproblems.',
        code: `def fibonacci(n, memo={}):
    if n in memo:
        return memo[n]
    if n <= 2:
        return 1
    memo[n] = fibonacci(n-1, memo) + fibonacci(n-2, memo)
    return memo[n]`
    }
};

// Mock API functions (for testing without backend)
const mockAPI = {
    users: [], // Store registered users

    async login(credentials) {
        await new Promise(resolve => setTimeout(resolve, 1000)); // Simulate delay
        console.log('Mock Login attempt:', credentials);
        
        // Check registered users first
        const user = this.users.find(u => 
            (u.email === credentials.emailOrUsername || u.username === credentials.emailOrUsername) &&
            u.password === credentials.password
        );
        
        if (user) {
            const userData = {
                id: user.id,
                fullName: user.fullName,
                email: user.email,
                username: user.username,
                progress: Object.keys(topicData).map(topic => ({
                    topicName: topic,
                    isCompleted: Math.random() > 0.7 // Some random progress
                })),
                overallProgressPercentage: 30
            };
            console.log('Mock Login successful:', userData);
            return { success: true, data: { token: "mock-token-" + user.id, user: userData } };
        }
        
        // Fallback - accept any credentials for demo
        if (credentials.emailOrUsername && credentials.password) {
            const userData = {
                id: 999,
                fullName: "Demo User",
                email: credentials.emailOrUsername,
                username: "demouser",
                progress: Object.keys(topicData).map(topic => ({
                    topicName: topic,
                    isCompleted: Math.random() > 0.6
                })),
                overallProgressPercentage: 40
            };
            console.log('Mock Login successful (demo):', userData);
            return { success: true, data: { token: "mock-token-demo", user: userData } };
        }
        
        throw new Error("Invalid credentials");
    },

    async register(userData) {
        await new Promise(resolve => setTimeout(resolve, 1000));
        console.log('Mock Register attempt:', userData);
        
        // Check if user already exists
        const existingUser = this.users.find(u => 
            u.email === userData.email || u.username === userData.username
        );
        
        if (existingUser) {
            throw new Error("User already exists");
        }
        
        // Create new user
        const newUser = {
            id: this.users.length + 1,
            fullName: userData.fullName,
            email: userData.email,
            username: userData.username,
            password: userData.password, // In real app, this would be hashed
            progress: Object.keys(topicData).map(topic => ({
                topicName: topic,
                isCompleted: false
            })),
            overallProgressPercentage: 0
        };
        
        this.users.push(newUser);
        console.log('Mock Register successful:', newUser);
        console.log('All registered users:', this.users);
        
        return { 
            success: true, 
            data: { 
                token: "mock-token-" + newUser.id, 
                user: newUser 
            } 
        };
    },

    async updateProgress(topicName, isCompleted) {
        await new Promise(resolve => setTimeout(resolve, 500));
        console.log('Mock Progress update:', topicName, isCompleted);
        return { success: true };
    }
};

// API Helper
async function apiCall(endpoint, method = 'GET', data = null) {
    if (USE_MOCK_API) {
        if (endpoint === '/auth/login' && method === 'POST') {
            return await mockAPI.login(data);
        }
        if (endpoint === '/auth/register' && method === 'POST') {
            return await mockAPI.register(data);
        }
        if (endpoint === '/progress/update' && method === 'POST') {
            return await mockAPI.updateProgress(data.topicName, data.isCompleted);
        }
    }

    const config = {
        method,
        headers: { 'Content-Type': 'application/json' }
    };

    if (authToken) {
        config.headers['Authorization'] = `Bearer ${authToken}`;
    }

    if (data) {
        config.body = JSON.stringify(data);
    }

    const response = await fetch(`${API_BASE_URL}${endpoint}`, config);
    const result = await response.json();
    
    if (!response.ok) {
        throw new Error(result.message || 'API call failed');
    }
    
    return result;
}

// Error handling
function showError(elementId, message) {
    const element = document.getElementById(elementId);
    if (element) {
        element.textContent = message;
        element.classList.remove('hidden');
    }
}

function clearErrors() {
    document.querySelectorAll('.error-message').forEach(el => {
        el.classList.add('hidden');
        el.textContent = '';
    });
}

// Authentication
async function doLogin(event) {
    event.preventDefault();
    
    const loginBtn = document.getElementById('loginBtn');
    const loginText = document.getElementById('loginText');
    const loginLoader = document.getElementById('loginLoader');
    
    console.log('Login form submitted');
    clearErrors();
    
    const emailOrUsername = document.getElementById('loginEmail').value.trim();
    const password = document.getElementById('loginPassword').value;
    
    console.log('Login attempt:', emailOrUsername, 'password: ***');
    
    // Basic validation
    if (!emailOrUsername) {
        showError('loginEmailError', 'Email or username is required');
        return;
    }
    if (!password) {
        showError('loginPasswordError', 'Password is required');
        return;
    }
    
    loginBtn.disabled = true;
    loginText.classList.add('hidden');
    loginLoader.classList.remove('hidden');
    
    try {
        console.log('Calling login API...');
        const response = await apiCall('/auth/login', 'POST', { emailOrUsername, password });
        
        console.log('Login API response:', response);
        
        if (response.success) {
            authToken = response.data.token;
            currentUser = response.data.user;
            localStorage.setItem('authToken', authToken);
            localStorage.setItem('currentUser', JSON.stringify(currentUser));
            
            console.log('Login successful, user data saved:', currentUser);
            
            loadUserProgress();
            
            // Show success message
            alert(`🎉 Welcome back, ${currentUser.fullName}!`);
            
            goToDashboard();
        } else {
            console.log('Login failed:', response.message);
            showError('loginError', response.message);
        }
    } catch (error) {
        console.error('Login error:', error);
        showError('loginError', error.message || 'Login failed. Please check your credentials.');
    } finally {
        loginBtn.disabled = false;
        loginText.classList.remove('hidden');
        loginLoader.classList.add('hidden');
    }
}

async function doSignup(event) {
    event.preventDefault();
    
    const signupBtn = document.getElementById('signupBtn');
    const signupText = document.getElementById('signupText');
    const signupLoader = document.getElementById('signupLoader');
    
    console.log('Signup form submitted');
    clearErrors();
    
    const fullName = document.getElementById('signupFullName').value.trim();
    const email = document.getElementById('signupEmail').value.trim();
    const username = document.getElementById('signupUsername').value.trim();
    const password = document.getElementById('signupPassword').value;
    const confirmPassword = document.getElementById('signupConfirmPassword').value;
    
    console.log('Signup data:', { fullName, email, username, password: '***' });
    
    // Enhanced validation
    let hasErrors = false;
    
    if (!fullName || fullName.length < 2) {
        showError('signupFullNameError', 'Full name must be at least 2 characters');
        hasErrors = true;
    }
    
    // Email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email) {
        showError('signupEmailError', 'Email is required');
        hasErrors = true;
    } else if (!emailRegex.test(email)) {
        showError('signupEmailError', 'Please enter a valid email address');
        hasErrors = true;
    }
    
    // Username validation
    if (!username) {
        showError('signupUsernameError', 'Username is required');
        hasErrors = true;
    } else if (username.length < 3) {
        showError('signupUsernameError', 'Username must be at least 3 characters');
        hasErrors = true;
    } else if (!/^[a-zA-Z0-9_]+$/.test(username)) {
        showError('signupUsernameError', 'Username can only contain letters, numbers, and underscores');
        hasErrors = true;
    }
    
    // Password validation
    if (!password) {
        showError('signupPasswordError', 'Password is required');
        hasErrors = true;
    } else if (password.length < 6) {
        showError('signupPasswordError', 'Password must be at least 6 characters');
        hasErrors = true;
    }
    
    // Confirm password validation
    if (!confirmPassword) {
        showError('signupConfirmPasswordError', 'Please confirm your password');
        hasErrors = true;
    } else if (password !== confirmPassword) {
        showError('signupConfirmPasswordError', 'Passwords do not match');
        hasErrors = true;
    }
    
    if (hasErrors) {
        console.log('Signup validation failed');
        return;
    }
    
    // Show loading state
    signupBtn.disabled = true;
    signupText.classList.add('hidden');
    signupLoader.classList.remove('hidden');
    
    try {
        console.log('Calling signup API...');
        const response = await apiCall('/auth/register', 'POST', {
            fullName, email, username, password, confirmPassword
        });
        
        console.log('Signup API response:', response);
        
        if (response && response.success) {
            console.log('Signup successful!');
            
            // Clear the form
            document.getElementById('signupForm').reset();
            
            // Show success message
            alert(`🎉 Account created successfully!\n\nYou can now login with:\nEmail: ${email}\nUsername: ${username}\nPassword: [your password]`);
            
            // Redirect to login page
            goToLogin();
            
            // Pre-fill login form with the email they just used
            setTimeout(() => {
                const loginEmailField = document.getElementById('loginEmail');
                if (loginEmailField) {
                    loginEmailField.value = email;
                    loginEmailField.focus();
                }
            }, 100);
            
        } else {
            console.log('Signup failed:', response?.message || 'Unknown error');
            showError('signupError', response?.message || 'Registration failed. Please try again.');
        }
    } catch (error) {
        console.error('Signup error:', error);
        showError('signupError', error.message || 'Registration failed. Please try again.');
    } finally {
        // Reset button state
        signupBtn.disabled = false;
        signupText.classList.remove('hidden');
        signupLoader.classList.add('hidden');
    }
}

function logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('currentUser');
    authToken = null;
    currentUser = null;
    userProgress = {};
    goToLogin();
}

// Progress Management
function loadUserProgress() {
    if (currentUser && currentUser.progress) {
        userProgress = {};
        currentUser.progress.forEach(item => {
            userProgress[item.topicName] = item.isCompleted;
        });
    }
}

async function updateTopicProgress(topicName, isCompleted) {
    try {
        if (USE_MOCK_API) {
            userProgress[topicName] = isCompleted;
            updateProgressUI();
            return;
        }
        
        await apiCall('/progress/update', 'POST', {
            topicName,
            isCompleted
        });
        
        userProgress[topicName] = isCompleted;
        updateProgressUI();
    } catch (error) {
        console.error('Failed to update progress:', error);
    }
}

function updateProgressUI() {
    const totalTopics = Object.keys(topicData).length;
    const completedTopics = Object.values(userProgress).filter(Boolean).length;
    const percentage = totalTopics > 0 ? Math.round((completedTopics / totalTopics) * 100) : 0;
    
    // Update overall progress
    const progressElement = document.getElementById('overallProgress');
    const progressBarElement = document.getElementById('overallProgressBar');
    
    if (progressElement) progressElement.textContent = percentage + '%';
    if (progressBarElement) progressBarElement.style.width = percentage + '%';
    
    // Update dashboard cards
    updateDashboardCards();
    
    // Update profile progress
    updateProfileProgress();
}

function updateDashboardCards() {
    Object.keys(topicData).forEach(topicName => {
        const card = document.querySelector(`[data-topic="${topicName}"]`);
        const checkbox = document.querySelector(`[data-topic-checkbox="${topicName}"]`);
        
        if (card && checkbox) {
            const isCompleted = userProgress[topicName] || false;
            checkbox.checked = isCompleted;
            
            if (isCompleted) {
                card.classList.add('completed');
            } else {
                card.classList.remove('completed');
            }
        }
    });
}

function updateProfileProgress() {
    const dataStructures = ['Trees', 'Linked Lists', 'Hash Tables', 'Graphs'];
    const algorithms = ['Sorting', 'Searching', 'Traversal', 'Dynamic Programming'];
    
    updateProgressSection('dataStructuresProgress', dataStructures);
    updateProgressSection('algorithmsProgress', algorithms);
}

function updateProgressSection(sectionId, topics) {
    const section = document.getElementById(sectionId);
    if (!section) return;
    
    section.innerHTML = topics.map(topic => {
        const isCompleted = userProgress[topic] || false;
        return `
            <div class="checklist-item">
                <input class="checklist-checkbox" type="checkbox" ${isCompleted ? 'checked' : ''} 
                       onchange="updateTopicProgress('${topic}', this.checked)">
                <label class="checklist-label ${isCompleted ? 'completed-label' : ''}">${topic}</label>
            </div>
        `;
    }).join('');
}

// UI Functions
function renderDashboardCards() {
    const dataStructures = Object.entries(topicData).filter(([name, data]) => data.category === 'dataStructures');
    const algorithms = Object.entries(topicData).filter(([name, data]) => data.category === 'algorithms');
    
    renderCardGrid('dataStructuresGrid', dataStructures);
    renderCardGrid('algorithmsGrid', algorithms);
    
    updateDashboardCards();
}

function renderCardGrid(gridId, topics) {
    const grid = document.getElementById(gridId);
    if (!grid) return;
    
    grid.innerHTML = topics.map(([topicName, topicInfo]) => {
        const isCompleted = userProgress[topicName] || false;
        return `
            <div class="card ${isCompleted ? 'completed' : ''}" data-topic="${topicName}" onclick="showTopic('${topicName}')">
                <input type="checkbox" class="card-checkbox" data-topic-checkbox="${topicName}" 
                       ${isCompleted ? 'checked' : ''} 
                       onclick="event.stopPropagation(); updateTopicProgress('${topicName}', this.checked)">
                <div class="card-icon">${topicInfo.icon}</div>
                <div class="card-title">${topicName}</div>
                <div class="card-description">${topicInfo.intro}</div>
            </div>
        `;
    }).join('');
}

function showUserInfo() {
    if (currentUser) {
        const userNameElement = document.getElementById('userName');
        const userEmailElement = document.getElementById('userEmail');
        
        if (userNameElement) userNameElement.textContent = `Welcome, ${currentUser.fullName}!`;
        if (userEmailElement) userEmailElement.textContent = currentUser.email;
    }
}

// Navigation Functions
function hideAll() {
    const pages = ['loginPage', 'signupPage', 'dashboardPage', 'topicPage', 'profilePage'];
    pages.forEach(pageId => {
        document.getElementById(pageId).classList.add('hidden');
    });
}

function goToSignup() {
    console.log('Navigating to signup page...');
    hideAll();
    document.getElementById('signupPage').classList.remove('hidden');
    
    // Re-setup event listeners when page changes
    setTimeout(setupEventListeners, 50);
}

function goToLogin() {
    console.log('Navigating to login page...');
    hideAll();
    document.getElementById('loginPage').classList.remove('hidden');
    
    // Re-setup event listeners when page changes
    setTimeout(setupEventListeners, 50);
}

function goToDashboard() {
    hideAll();
    document.getElementById('dashboardPage').classList.remove('hidden');
    showUserInfo();
    renderDashboardCards();
    updateProgressUI();
}

function goToProfile() {
    hideAll();
    document.getElementById('profilePage').classList.remove('hidden');
    updateProgressUI();
}

async function showTopic(topicName) {
    const topic = topicData[topicName];
    if (!topic) return;
    
    document.getElementById('topicTitle').textContent = topicName;
    document.getElementById('currentTopic').textContent = topicName;
    document.getElementById('topicIntro').textContent = topic.intro;
    document.getElementById('topicDescription').textContent = topic.description;
    document.getElementById('topicCode').textContent = topic.code;
    
    // Update avatar in topic page header
    const topicAvatar = document.getElementById('topicAvatar');
    if (topicAvatar && currentUser && currentUser.avatar) {
        topicAvatar.textContent = currentUser.avatar;
    }
    
    // Load and display diagram
    const diagramContainer = document.getElementById('topicDiagram');
    if (diagramContainer) {
        // Show loading state
        diagramContainer.innerHTML = '<div class="loading">Loading diagram...</div>';
        diagramContainer.style.display = 'block';
        
        try {
            // Fetch diagram data from API
            const response = await apiCall('/topics');
            if (response.success && response.data) {
                const topicWithDiagram = response.data.find(t => t.name === topicName);
                if (topicWithDiagram && topicWithDiagram.diagramData) {
                    diagramContainer.innerHTML = topicWithDiagram.diagramData;
                } else {
                    diagramContainer.style.display = 'none';
                }
            } else {
                diagramContainer.style.display = 'none';
            }
        } catch (error) {
            console.error('Error loading diagram:', error);
            diagramContainer.style.display = 'none';
        }
    }
    
    hideAll();
    document.getElementById('topicPage').classList.remove('hidden');
}

// Event Listeners - Fixed
function setupEventListeners() {
    console.log('Setting up event listeners...');
    
    // Login form
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.removeEventListener('submit', doLogin); // Remove existing
        loginForm.addEventListener('submit', doLogin);
        console.log('Login form listener attached');
    }
    
    // Signup form
    const signupForm = document.getElementById('signupForm');
    if (signupForm) {
        signupForm.removeEventListener('submit', doSignup); // Remove existing
        signupForm.addEventListener('submit', doSignup);
        console.log('Signup form listener attached');
    }
    
    // Test button clicks as backup
    const loginBtn = document.getElementById('loginBtn');
    if (loginBtn) {
        loginBtn.onclick = function(e) {
            console.log('Login button clicked directly');
            e.preventDefault();
            doLogin(e);
        };
    }
    
    const signupBtn = document.getElementById('signupBtn');
    if (signupBtn) {
        signupBtn.onclick = function(e) {
            console.log('Signup button clicked directly');
            e.preventDefault();
            doSignup(e);
        };
    }
}

// Auto-login check - Modified to always go to login
function checkAutoLogin() {
    // Clear any existing authentication data to force login
    localStorage.removeItem('authToken');
    localStorage.removeItem('currentUser');
    authToken = null;
    currentUser = null;
    userProgress = {};
    
    // Always redirect to login page
    goToLogin();
}

// Search functionality
function performSearch() {
    const searchInput = document.getElementById('searchInput');
    const searchQuery = searchInput.value.toLowerCase().trim();
    
    const allCards = document.querySelectorAll('.card');
    let visibleCount = 0;
    
    // Remove any existing no results messages
    const existingNoResults = document.querySelectorAll('.no-results');
    existingNoResults.forEach(el => el.remove());
    
    if (searchQuery === '') {
        // Show all cards if search is empty
        allCards.forEach(card => {
            card.style.display = '';
        });
        return;
    }
    
    allCards.forEach(card => {
        const topicName = card.getAttribute('data-topic');
        const topic = topicData[topicName];
        
        if (!topic) {
            card.style.display = 'none';
            return;
        }
        
        const searchableText = `${topicName} ${topic.intro} ${topic.description} ${topic.category}`.toLowerCase();
        
        if (searchableText.includes(searchQuery)) {
            card.style.display = '';
            visibleCount++;
        } else {
            card.style.display = 'none';
        }
    });
    
    // Show no results message if needed
    if (visibleCount === 0) {
        const dataStructuresGrid = document.getElementById('dataStructuresGrid');
        const noResultsMessage = document.createElement('div');
        noResultsMessage.className = 'no-results';
        noResultsMessage.style.cssText = 'grid-column: 1/-1; text-align: center; padding: 2rem; color: #666;';
        noResultsMessage.innerHTML = `
            <p>No results found for "<strong>${searchQuery}</strong>"</p>
            <p style="margin-top: 1rem; font-size: 0.9rem;">Try searching for topics like "tree", "sort", "graph", or "dynamic"</p>
        `;
        
        if (dataStructuresGrid) {
            dataStructuresGrid.appendChild(noResultsMessage);
        }
    }
}

// Avatar functionality
function selectAvatar(avatar) {
    // Update the selected avatar
    const avatarOptions = document.querySelectorAll('.avatar-option');
    avatarOptions.forEach(option => {
        if (option.textContent === avatar) {
            option.style.border = '2px solid #28a745';
            option.style.background = '#e8f5e9';
        } else {
            option.style.border = '2px solid transparent';
            option.style.background = '#f8f9fa';
        }
    });
    
    // Update user data if logged in
    if (currentUser) {
        currentUser.avatar = avatar;
        localStorage.setItem('currentUser', JSON.stringify(currentUser));
        
        // Update all avatar displays
        updateAvatarDisplays(avatar);
    }
}

function updateAvatarDisplays(avatar) {
    // Update dashboard avatar
    const dashboardAvatar = document.querySelector('.user-avatar');
    if (dashboardAvatar) {
        dashboardAvatar.textContent = avatar;
    }
    
    // Update any other avatar displays
    const profileAvatars = document.querySelectorAll('.profile-avatar');
    profileAvatars.forEach(el => {
        el.textContent = avatar;
    });
}

// Initialize app - Updated
document.addEventListener('DOMContentLoaded', function() {
    console.log('🚀 CodeCraft app loaded!');
    console.log('Setting up application...');
    
    // Setup event listeners first
    setupEventListeners();
    
    // Add a small delay to ensure everything is ready
    setTimeout(() => {
        checkAutoLogin();
        console.log('App initialization complete');
    }, 100);
});

// Additional debugging - Test functions
window.testLogin = function() {
    console.log('Testing login function...');
    doLogin({ preventDefault: () => {} });
};

window.testSignup = function() {
    console.log('Testing signup function...');
    doSignup({ preventDefault: () => {} });
};

window.debugApp = function() {
    console.log('=== DEBUG INFO ===');
    console.log('Current user:', currentUser);
    console.log('Auth token:', authToken);
    console.log('User progress:', userProgress);
    console.log('Mock API users:', mockAPI.users);
    console.log('Login form element:', document.getElementById('loginForm'));
    console.log('Signup form element:', document.getElementById('signupForm'));
};