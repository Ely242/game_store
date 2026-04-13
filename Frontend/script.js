const API_BASE = 'http://localhost:5048';
let allGames = [];
let allGenres = [];
let editingGameId = null;

function formatReleaseDate(releaseDate) {
    if (!releaseDate) {
        return 'Unknown';
    }

    return new Date(releaseDate).toLocaleDateString();
}

async function fetchGameDetails(id) {
    const response = await fetch(`${API_BASE}/games/${id}`);

    if (!response.ok) {
        throw new Error('Failed to load game details');
    }

    return response.json();
}

// Tab switching
function switchTab(event, tabName) {
    document.querySelectorAll('.tab-content').forEach(el => el.classList.remove('active'));
    document.querySelectorAll('.tab-btn').forEach(el => el.classList.remove('active'));
    
    document.getElementById(tabName).classList.add('active');
    event.target.classList.add('active');
    
    if (tabName === 'games') {
        loadGames();
    } else {
        loadGenres();
    }
}

// Show notification
function showNotification(message, type = 'success') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type}`;
    alertDiv.textContent = message;
    
    const container = document.querySelector('.container');
    container.insertBefore(alertDiv, container.firstChild);
    
    setTimeout(() => alertDiv.remove(), 4000);
}

// Load all games
async function loadGames() {
    try {
        const response = await fetch(`${API_BASE}/games`);
        if (!response.ok) throw new Error('Failed to load games');
        allGames = await response.json();
        renderGames();
    } catch (error) {
        showNotification('Error loading games: ' + error.message, 'error');
    }
}

// Render games
function renderGames() {
    const gamesList = document.getElementById('gamesList');
    
    if (allGames.length === 0) {
        gamesList.innerHTML = '<div class="empty-state" style="grid-column: 1/-1;"><div class="empty-state-icon">🎮</div><p>No games yet. Add one to get started!</p></div>';
        return;
    }

    gamesList.innerHTML = allGames.map(game => `
        <div class="game-card">
            <h3>${game.name}</h3>
            <div class="game-card-info">
                <strong>Genre:</strong> ${game.genre || 'Unknown'}
            </div>
            <div class="game-card-info">
                <strong>Price:</strong> $${parseFloat(game.price).toFixed(2)}
            </div>
            <div class="game-card-info">
                <strong>Release:</strong> ${formatReleaseDate(game.releaseDate)}
            </div>
            <div class="game-card-actions">
                <button class="btn-secondary" onclick="editGame(${game.id})">Edit</button>
                <button class="btn-danger" onclick="deleteGame(${game.id})">Delete</button>
            </div>
        </div>
    `).join('');
}

// Add game
async function addGame(event) {
    event.preventDefault();
    
    const title = document.getElementById('gameTitle').value;
    const genreId = parseInt(document.getElementById('gameGenre').value);
    const price = parseFloat(document.getElementById('gamePrice').value);
    const releaseDate = document.getElementById('gameReleaseDate').value;

    if (!title || !genreId || !price || !releaseDate) {
        showNotification('Please fill in all fields', 'error');
        return;
    }

    try {
        const method = editingGameId ? 'PUT' : 'POST';
        const url = editingGameId ? `${API_BASE}/games/${editingGameId}` : `${API_BASE}/games`;

        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                title,
                genreId,
                price,
                releaseDate
            })
        });

        if (!response.ok) throw new Error('Failed to save game');

        showNotification(editingGameId ? 'Game updated successfully!' : 'Game added successfully!', 'success');
        document.getElementById('gameForm').reset();
        editingGameId = null;
        loadGames();
    } catch (error) {
        showNotification('Error saving game: ' + error.message, 'error');
    }
}

// Edit game
async function editGame(id) {
    try {
        const game = await fetchGameDetails(id);

        editingGameId = id;
        document.getElementById('gameTitle').value = game.title;
        document.getElementById('gameGenre').value = game.genreId;
        document.getElementById('gamePrice').value = game.price;
        document.getElementById('gameReleaseDate').value = game.releaseDate ? game.releaseDate.split('T')[0] : '';

        window.scrollTo({ top: 0, behavior: 'smooth' });
    } catch (error) {
        showNotification('Error loading game details: ' + error.message, 'error');
    }
}

// Delete game
async function deleteGame(id) {
    if (!confirm('Are you sure you want to delete this game?')) return;

    try {
        const response = await fetch(`${API_BASE}/games/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Failed to delete game');

        showNotification('Game deleted successfully!', 'success');
        loadGames();
    } catch (error) {
        showNotification('Error deleting game: ' + error.message, 'error');
    }
}

// Load genres
async function loadGenres() {
    try {
        const response = await fetch(`${API_BASE}/genres`);
        if (!response.ok) throw new Error('Failed to load genres');
        allGenres = await response.json();
        renderGenres();
        populateGenreSelect();
    } catch (error) {
        showNotification('Error loading genres: ' + error.message, 'error');
    }
}

// Render genres
function renderGenres() {
    const genresList = document.getElementById('genresList');
    
    if (allGenres.length === 0) {
        genresList.innerHTML = '<div class="empty-state" style="grid-column: 1/-1;"><div class="empty-state-icon">📚</div><p>No genres yet.</p></div>';
        return;
    }

    genresList.innerHTML = allGenres.map(genre => `
        <div class="genre-item">
            <strong>${genre.name}</strong>
            <button class="btn-danger" onclick="deleteGenre(${genre.id})">Delete</button>
        </div>
    `).join('');
}

// Populate genre select
function populateGenreSelect() {
    const select = document.getElementById('gameGenre');
    const currentValue = select.value;
    
    select.innerHTML = '<option value="">Select a genre...</option>' + 
        allGenres.map(genre => `<option value="${genre.id}">${genre.name}</option>`).join('');
    
    if (currentValue) select.value = currentValue;
}

// Add genre
async function addGenre(event) {
    event.preventDefault();
    
    const name = document.getElementById('genreName').value;

    if (!name.trim()) {
        showNotification('Please enter a genre name', 'error');
        return;
    }

    try {
        const response = await fetch(`${API_BASE}/genres`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name })
        });

        if (!response.ok) throw new Error('Failed to add genre');

        showNotification('Genre added successfully!', 'success');
        document.getElementById('genreForm').reset();
        loadGenres();
    } catch (error) {
        showNotification('Error adding genre: ' + error.message, 'error');
    }
}

// Delete genre
async function deleteGenre(id) {
    if (!confirm('Are you sure? Games using this genre may be affected.')) return;

    try {
        const response = await fetch(`${API_BASE}/genres/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Failed to delete genre');

        showNotification('Genre deleted successfully!', 'success');
        loadGenres();
    } catch (error) {
        showNotification('Error deleting genre: ' + error.message, 'error');
    }
}

// Initialize on load
window.addEventListener('load', () => {
    loadGames();
    loadGenres();
});
