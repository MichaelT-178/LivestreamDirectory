window.addEventListener('load', () => {

    const title = localStorage.getItem("theTitle");
    const artist = localStorage.getItem("theArtist");
    const other_artists = localStorage.getItem("theOtherArt");
    const appears = localStorage.getItem("Appears");
    let instruments = localStorage.getItem("theInstruments");
    const image = `../pics/${localStorage.getItem("theImage")}`;
    const theLinks = localStorage.getItem("theLinks");

    let theTitle = title + (title.includes("Session #") ? " (Check comments for full timestamp)" : "");

    const mediaQuery = window.matchMedia('(min-device-width: 375px) and (max-device-width: 812px)');

    let numberOfH;
    let hString;
    
    const instrumentList = instruments.split(",");
    const longestString = findLongestStringLength(instrumentList);

    if (!mediaQuery.matches) {
        if (theTitle.length < 20 && artist.length < 20 && other_artists.length < 20 && instruments.length < 20 && longestString < 22) {
            const songInfoHeading = document.getElementById('songInfo');
            const newMarginLeft = songInfoHeading.style.marginLeft - 100;

            songInfoHeading.style.marginLeft = newMarginLeft + 'px';
        }
    } else {
        if (theTitle.length < 30 && artist.length < 30 && other_artists.length < 30 && longestString < 30) {
            const songInfoHeading = document.getElementById('songInfo');
            const newMarginLeft = songInfoHeading.style.marginLeft - 50;

            songInfoHeading.style.marginLeft = newMarginLeft + 'px';
        } 
    }

    document.getElementById('result-title').innerHTML = ": " + theTitle;
    document.getElementById('result-artist').innerText = ": " + artist;
    document.getElementById('result-otherart').innerText = ": " + (other_artists || "N/A");
    document.getElementById('result-instruments').innerHTML = ": " + instruments;

    const marginLeft = instrumentList.length > 2 ? 34 : 10;

    const instrumentStyling = "font-size: 24px;" + 
                              "color: lightBlue;" +
                              "text-align: left;" +
                              `margin-left: ${marginLeft}px;` +
                              "margin-bottom: 9px;";
                              
    if (mediaQuery.matches) {
        console.log("HELLO WORLD")
        document.getElementById('result-instruments').innerHTML = "";
        
        document.getElementById('result-instrument').innerHTML = instrumentList.map((instrument) => 
        ` <div style="${instrumentStyling}"> • ${instrument} </div>`
        ).join(''); 
    }; 
    
    window.addEventListener('error', () => {
        console.error("Image not found");
        console.error("Image name: " + image);
    }, true);

    document.getElementById("pic1").src = image;

    web = appears.split(",");

    const alllinks = theLinks.split(" , ").map((link, i) => 
    `<div style="margin: 10px 0;">• <a href="${link}" style= "font-size: 25px;text-decoration: none;">${web[i]}</a></div>`
    ).join('');

    document.getElementById('result-links').innerHTML = alllinks;

});


const findLongestStringLength = (arr) => {
    let longest = '';
    
    for (let i = 0; i < arr.length; i++) {
      if (arr[i].length > longest.length) {
        longest = arr[i];
      }
    }
    
    return longest.length;
  }