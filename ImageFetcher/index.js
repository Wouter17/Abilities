import fs from "fs";
import * as download from "image-downloader";

const header = 'https://ddragon.leagueoflegends.com/cdn/12.15.1/img/spell/';

const stream = fs.createWriteStream('failed.txt', {flags: 'a'})

fs.writeFileSync('./failed.txt', '', null);
const championNamesFile = fs.readFileSync('./champions.txt', {encoding:'utf8', flag:'r'});
const champions = championNamesFile.split(', ').map(name => {
    if (name === "Fiddlesticks") return 'FiddleSticks';
    if (name === "Renata Glasc") return 'Renata';
    if (name === "Nunu and Willump") return 'Nunu';
    
    if (!(name === "Aurelion Sol" || name === "Dr. Mundo" || name === "Rek'Sai" || name === "Kog'Maw" || name === "Tahm Kench" || name === "Xin Zhao")) //because they are special somehow
        name = name.slice(0, 1) + name.slice(1).toLowerCase();
    
    return name.replace(/\s|'|\./g, '');
});

const correctMap = new Map();
const manualFetchChampions = fs.readFileSync('./manualFetching.txt', {encoding:'utf8', flag:'r'});
manualFetchChampions.split(/\r?\n/).forEach(combination => {
    const args = combination.split(', ');
    correctMap.set(args[1], args[0]);
});

const manualIncorrectFetchChampions = fs.readFileSync('./manualFetchingWrongNames.txt', {encoding:'utf8', flag:'r'});
manualIncorrectFetchChampions.split(/\r?\n/).forEach(combination => {
    const args = combination.split(', ');
    correctMap.set(args[1], args[0]);
});

Promise.all(champions.map(async champion => {
    const images = [];
    for (const image of ['Q', 'W', 'E', 'R']){
        images.push(
            download.image({
                url: header + champion + image + '.png',
                dest: "../../abilities", //Relative directs from the module for some reason
            })
                .then(({ filename }) => {
                    console.log('Saved', filename); // saved to /path/to/dest/photo.jpg
                })
                .catch(() => {
                        if (correctMap.has(`${champion} ${image}`)){
                            download.image({
                                url: header + correctMap.get(`${champion} ${image}`) + '.png',
                                dest: `../../abilities/${champion}${image}.png`, //Relative directs from the module for some reason
                            })
                                .then(({ filename }) => {
                                    console.log('Saved', filename); // saved to /path/to/dest/photo.jpg
                                })
                                .catch(() => 
                                    stream.write(`Failed for ${champion} ${image} with ${correctMap.get(`${champion} ${image}`)}\n`)
                                )
                        }
                        else stream.write(`Failed for ${champion} ${image}\n`);
                    }
                )
        );
    }
    return images;
}))

stream.on("end", function() {
    stream.end();
});

console.log("Done");