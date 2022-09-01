const search = document.getElementById('search');
const matchList = document.getElementById('match-list');
let otherMatches = [];

const searchSongs = async searchText => {

    const res = await fetch('./json/song_list.json');
    const songs = await res.json();

    let matches = songs.filter(song => {
        const regex = searchText.toLowerCase().trim();
        return song.Title.toLowerCase().match(regex) || song.Artist.toLowerCase().match(regex) || 
               song.Other_Artists.toLowerCase().match(regex) || song.Instruments.toLowerCase().match(regex) || 
               song.Other.toLowerCase().match(regex);
    });
    
    const l = document.getElementById('link');
    const l2 = document.getElementById('helplink');

    l.style.display =  (matches.length === 0 || searchText.length === 0) ? "flex" : "none";
    l2.style.display = (matches.length === 0 || searchText.length === 0) ? "flex" : "none";

    if (searchText.length === 0 || matches.length === 0) {
        matches = [];
        const thehtml = 'No results for: "' + searchText + '"';
        matchList.innerText = (searchText === "") ? '' : thehtml;
    };

    outputHtml(matches);
    otherMatches = matches;
};


const outputHtml = matches => {
    if (matches.length > 0) {
        const html = matches.map((match, i) => 
       
        `<div class="card card-body mb-1" data-id="${i}">
        <a href="indexTwo.html" style="text-decoration:none;">
            <button type="button"  style="background-color:#222; width: 100%; overflow: hidden;">

               
                <h4>${match.Title} by <span class="text-primary">
                ${match.Artist}</span></h4>
                <small style="color:#FFFFFF">Instruments: ${match.Instruments} / Other Artists: 
                ${(match.Other_Artists === "") ? "N/A" : match.Other_Artists}</small>
                

                </button>
                </a>
            </div>

        `).join('');
    matchList.innerHTML = html;
  };
};

//46D3F6

outputHtml(otherMatches);

matchList.addEventListener('click', (e) => {
    const button = e.target.closest('button');

    if (button) {
        const card = button.closest('.card');
        const id = card.dataset.id;
        matchList.innerText = "";   

        localStorage.setItem("theTitle", otherMatches[id].Title);
        localStorage.setItem("theArtist", otherMatches[id].Artist);
        localStorage.setItem("theOtherArt", otherMatches[id].Other_Artists);
        localStorage.setItem("Appears", otherMatches[id].Appearances);
        localStorage.setItem("theInstruments", otherMatches[id].Instruments);
        localStorage.setItem("theImage", otherMatches[id].Image);
        localStorage.setItem("theLinks", otherMatches[id].Links);
        
        document.getElementById('search').value = "";
        document.getElementById('link').style.display = "flex";
        document.getElementById('helplink').style.display = "flex";
    };
});

search.addEventListener('input', () => searchSongs(search.value));
