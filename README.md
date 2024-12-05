# A11 Tiesinis Kodas (q = 2) Grandininis Dekodavimas

### Kodo parametrai
Kodo parametrus p = ErrorRate (klaidos tikimybė), k = CodeDimension (dimensija), n = CodeLength (kodo ilgis), G = GeneratorMatrix (generuojanti matrica) vartotojas turi nusatyti naudodamas `appsettings.json` failą:
```json
{
    "CodeParameters": {
        "ErrorRate": 0.0001,
        "CodeDimension": 4,
        "CodeLength": 7,
        "GeneratorMatrix": [
            [1, 0, 0, 0, 1, 1, 0],
            [0, 1, 0, 0, 1, 0, 1],
            [0, 0, 1, 0, 0, 1, 1],
            [0, 0, 0, 1, 1, 1, 1]
        ]
    }   
}
```
Taip pat, galima nenurodyti generuojančiosios matricos, t.y arba visiškai pašalinti:
```json
"GeneratorMatrix": [
    [1, 0, 0, 0, 1, 1, 0],
    [0, 1, 0, 0, 1, 0, 1],
    [0, 0, 1, 0, 0, 1, 1],
    [0, 0, 0, 1, 1, 1, 1]
]
```
arba nustatyti vertę `"GeneratorMatrix": null`, nenurodžius generuojančios matricos, programa pati sugeneruos atsitiktinę matricą atitinkančia G = \[Ik|P\] formą.

Pastaba: klaidos tikimybė rašoma su ".", nes JSON failo standartas nepalaiko kablelių sveikosios ir trupmeninės dalies išskyrimui ir neatsižvelgia į operacinės sistemos parametrus.
Pastaba: programa palaiko tik standartinio pavidalo generuojančią matricą, jeigu vartotojo pateikta matrica bus ne standartinio pavidalo, programa baigs darbą.

## Ataskaita

### Trūkumai
- Nerealizuotas 3 - čiasis scenarijus (paveiksliukų kodavimo ir dekodavimo);

### Trečiųjų šalių funkcijų bibliotekos
- `Microsoft.Extensions.Configuration.*` - naudojama nuskaityti programos konfigūraciją iš failo, įrašoma ir nurodoma `CT.csproj` faile, panaudojama `using Microsoft.Extensions.Configuration`;
- `System.Text` - ASCII ir Unicode simbolių kodavimo klasės; abstrakčios bazinės klasės, skirtos simbolių blokams konvertuoti į baitų blokus ir iš jų; ir pagalbinė klasė, kuri manipuliuoja „String“ objektais ir juos formatuoja nekurdama tarpinių „String“ egzempliorių, panaudojama `using System.Text`;
- Visi kiti `using` t.y `using CT.*` naudoja vidinius, šioje programoje aprašytus metodus.

### Programos paleidimas
Turint .NET 8 SDK, programą galima susikompiliuoti iš programinio kodo (src)[src/] naudojant `dotnet publish -c Release -r win-x64 --self-contained true` gautas katalogas `bin/Release/win-x64/` atitiks programinį kodą prisegtą užduoties įkėlimo metu.

Norint paleisti įprastai, reikia paleisti `CT.exe` failą, tačiau privalu įsitikinti, kad šalia jo yra `appsettings.json` failas, kuriame nurodyti aukščiau paminėti parametrai, kitaip programa netęs darbo.

Paleidus programą, vartotojas turi pasirinkti įvesties tipą, įvesti norimą vektorių ar tekstą (vektoriaus atveju, taip pat gali pasirinkti ar nori modifikuoti iš kanalo išėjusį vektorių).

### Kodo failų aprašymas
