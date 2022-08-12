import fs from "fs";

fs.writeFileSync('./template.txt', '', null);
const stream = fs.createWriteStream('template.txt', {flags: 'a'})

fs.readdir('./abilities', (err, files) => {
    files.forEach(file => stream.write(`${file.slice(0, -4)}: \n`))
});

stream.on("end", function() {
    stream.end();
});