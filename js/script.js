//Used for the fav_covers page

const userCardTemplate = document.querySelector("[data-user-template]")
const userCardContainer = document.querySelector("[data-user-cards-container]")
const searchInput = document.querySelector("[data-search]")

let users = [] 

searchInput.addEventListener("input", (e) => {
    const value = e.target.value.trim().toLowerCase();

    users.forEach(user => {
        const isVisible = user.song.toLowerCase().includes(value);
        user.element.classList.toggle("hide", !isVisible);
    })
});

fetch("../db_manager/json_files/fav_covers.json")
    .then(res => res.json())
    .then(data => {
        users = data.FavoriteCovers.map(user => {
            const card = userCardTemplate.content.cloneNode(true).children[0];
            const header = card.querySelector("[data-header]");
            const body = card.querySelector("[data-body]");
            
            header.textContent = user.Song
            body.textContent = user.Appearance  
            card.href = user.Link;

            userCardContainer.append(card)
            
            return {song: user.Song, element: card }
        })
        
    });
