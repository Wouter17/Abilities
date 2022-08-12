import fs from "fs";

const saveDir = './abilities_correct';

const championConversionFile = fs.readFileSync('./conversion.txt', {encoding:'utf8', flag:'r'});
const lines = championConversionFile.split(/\r?\n/);

const conversionMap = new Map();
lines.forEach(line => conversionMap.set(line.match(/^[^:]*/)[0], line.match(/(?<=:).+/)[0].trim()));

if (!fs.existsSync(saveDir))
    fs.mkdirSync(saveDir);

conversionMap.forEach((abilityName, abilityReference) => {
    fs.copyFile(`./abilities/${abilityReference}.png`, `${saveDir}/${abilityName}.png`, err => {
        if(err)
            console.log(`Failed for ${abilityName}, ${abilityReference} because ${err}`);
    });
});

console.log("Done");