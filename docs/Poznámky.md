# POZNÁMKY

... (myslím, že nejaké sú v aj zošite)

## P1. (UML, koncept. model)

- **Ponuky:** 
  - Zaviedol som triedu Ponuka, od ktorej môžu dediť všelijaké iné typy ponúk. Môžu sa ponúkať stroje, prídavné zariadenia... Ale takisto môže ísť aj o špeciálne typy ponúk, tzv. AukčnéPonuky (ktoré  takisto dedia od Ponuky). A od aukčných ponúk dedí AukčnáPonukaStrojov, ale takisto je tu môžne pridať ponuky niečoho iného, no zatial sú len stroje. (Rozšírenie do budúcna, čo ak do aukcie budeme chcieť pridať aj prídavné zariadenia?)
  - Ponuka by mohla byť abstraktná s abstraktnými props, napr. Názov. Každá zderivovaná trieda od Ponuky v sebe bude mať položku ktorú ponúka, napr PonukaPrídavnéhoZariadenia má v sebe PrídavnéZariadenie s Názvom, takže property Názov PonukyPrídavnéhoZariadenia sa implementuje za pomoci property Názov PrídavnéhoZariadenia.
- **Prihlasovací panel:** 
  - PrihlasovacíPanel má v sebe 2 verzie pre prihl. a neprihl. užívateľa (obs. 2 komponenty). Lepsie sa tak bude 
    stylovať si myslím (ak budú ako 2 komponenty).
- **Registrácia:** 
  - SkontrolujSíluHesla(heslo) pri Registrácii vráti int, bude séria "testov" (napr. či obsahuje
    špecialny znak akým je napr. '#', a ak áno pripočíta k síle hesla +1, nakoniec vráti sílu hesla- int.
    Podľa toho sa mu môže v UI ukázať aké má silné heslo).
- **Upozorňovače:** 
  - Upozorňovač bude mať Čas alebo Stroje budú mať Čas? Ak časy budú rovnaké pri všetkých strojoch, 
    tak môže ostať tak jak je, ALE keď každý stroj môže mať iný čas, tak treba prerobiť (by Stroj mal 
    čas i DátumPoslednejPrehliadky, by si vypočítal novú a Upozornovač by si len prešiel a pozrel 
    či už má poslať upozornenie alebo nie.).
    	-Vzdy 3 mesiace.
  - Neexistuje prístroj, ktorý by snímal motohodiny? By nám ich posielal a pri prekročení 
    by systém automaticky na to upozornil. To by bolo pohodlnejšie, nie?
    	-Je, ale nic z toho bo kto zaplati si take...
  - Možno sa bude dať zovšeobecniť *Upozorňovač*. Aby sa vložila podmienka len(predikát)
    a je to. Ale neviem či by podmienky niekedy neboli komplikované a vyždaovali sa props.
    (Neviem ako to vyjde, nemám toľko XP, osobitné dediace classy mi prídu viacej safe.)
  - *OdpočtovýUpozorňovač* aj ked nemá žiadne props, hodí sa osobitná classa, ak by sa chcelo zaviesť
    napr. také, že hodina pred koncom odošle každému záujemcovi email o tom, že už končí ponuka.
- **Stroje:** 
  - Stroje ako statické položky, preto lebo... 
    Všetky stroje sú rovnaké. Napr. keď je napr. UNC 070, tak všetky rovnaké budú. Navyše, nebudú pribúdať nové často. (Možno 1 za 5 rokov.)
- **Classa pre GLOBALne premenne a configy:** Zamýšľam staticku classu, ktorá by obsahovala Configy, Cesty, napr. ImagesRoute... Aby keď sa zmení cesta k obrázkom napr, tak aby som nemusel všade meniť ale iba na 1 mieste + ak by sa v budúcnosti chcel darkmod
- **Objednávky:** 
  - Statickou triedou Objednávky obaľujem triedy Objednávka. Trieda Objednávky bude mať statickú
    property List\<Objednávka\>[], takže každý admin bude vidieť všetky objednávky rovnako aktuálne.
    **! POZOR !** Nie je to hlúposť? Načo vytvárať static classu keď stačí static položka Objednávky v Administrátorovi.
    **! POZOR !** Ale zase čo ak sa bude chcieť pridať nejaká iná funkcionalita, ktorá by sa týkala objednávok, potom by dávalo zmysel mať classu ktorá by funckionality týkajúce sa Objednávok združovala. Takže hodí sa mať osobitnú classu. Kedže je to statická classa, nebude klásť veľké nároky na pamäť nakoľko nebude mať inštancie (nemalo b to ničom vadiť).
  - *Stavy objednávok (oprava stroja):* Stavy existuju preto, aby si zákazník vedel zobraziť čo sa s jeho strojom deje (v reálnom čase!). Momentálne tam mám len 3 stavy: VytvorenieObjednávky, Rozbor (pri tomto by boli zobrazené i nájdené chyby), PripravenéNaPrevzatie. Pôvodne bolo zamýšľaných viacej stavov, ale po premyslení som si povedal, že ak by ich bolo viac, bolo by to otravné ich furt zapisovať. Tieto 3 by mali byť dostatočne informatívne a zároveň nie otravné (pre admina) na zapisovanie. Prípadne sa môže v budúcnosti rozšíriť.
- **Poštár a Správy:** Napadlo mi, že Poštár bude posielať správy, budú existovať 2 overloady:
  1. na custom správy, klasicky nejaky string sa dá do metody a ten sa pošle
  2. na predpripravene spravy, to budu tie take co maju vzdy pevne danu kostru, len sa v nich menia napr mena uzivatelov, tie take automaticke spravy. Preto vytvaram aj classu pre Spravu, od ktorej mozu dedit nove classy, pre kazdu vzorovu spravu bude 1 trieda. Rozmyslal som ze parametre sa predaju constructoru a ten vytvori spravu. Ale teraz rozmyslam ze to je asi overkill pamatovy, je to zbytocne, staci mat metodu ktorej ked predas parametre ti zostavi pozadovanu spravu a kazdy typ/vzor spravy by bola nie 1 classa ale 1 metoda. takze Spravy by bola staticka trieda a b bolo napr. Spravy.BuildXYMsg(arg1, arg2) a vratilo by to string- vystavanu spravu/email.

---

## P2. (UML, koncept. model stále, už aj návrh databázy poriadnejšie)

- **Databáza:** Nemal by som ešte spraviť nejaku entitu Item od ktorej by dedili Stroje, Príd.Zariadenia atd.? Ale to by bol ďalší join, potrebujem to???
  - (https://youtu.be/ZS_kXvOeQ5Y?t=1168)
  - FaunaDB (multi-model databáza):
    - Free 5GB, ďalej sa platí len za to čo sa využíva
    - Vraj sa s ňou ľahko pracuje, ALE ja som s ňou nikdy nerobil...
    - Veľmi rýchla
    - ACID compliant (pri HW alebo network problému sa dáta nepoškodia)
    - Dá sa používať i so C#
  - MySQL (relational):
    - Free
    - Nerobil som síce s MySQL, ale s Microsoft SQL (mali by byť podobné)
    - ACID compliant
    - Dá sa používať i so C#
    - Neočakávam milióny údajov, takže joiny snáď až tak vadiť nebudú, ak by bolo údajov veľa, znamenalo by to, že ak by sme vybrali inú databázu, museli by sme platiť viac (za úložný priestor), ale ujo nechce platiť veľa -existuje hranica- a ak sa hranica prekročí, čo potom? Takže som za MySQL, pretože je zdarma bez obmedzenia úložného priestororu, a tiež preto lebo som s relačnou už pracoval.
  - *NoSQL* vraj dobré ak sa veľa číta, málo updatuje (lebo duplicitné dáta sú a všade ich potom musíš meniť).

- Static class **PrihláseníUžívatelia** bude obsahovať zoznam všetkých užívateľov, takže keď sa niektorí z užívateľov bude snažiť prihlásiť, skontroluje sa či už prihlásený je, ak áno, nájde sa v tomto zozname jeho inštancia a vráti sa mu jeho model, takto sa nebudeme musiet vždy dotazovať priamo z databázy a zároveň môže byť užívateľ prihlásený na viacerých PC. Ak by sa v zozname nenašiel, tak sa naťahajú dáta z databázy a vytvorí sa nová inštancia modelu.

**!!! Veľký UPDATE: !!!** 
- <u>Zrušili sa Objednávky</u> (kúpa stroja, prídavných zariadení, zemné práce, oprava stroja) ako entinty. Existovala myšlienka, že užívateľ si v profile bude môcť objednávky vylistovať a mal by o nich nejaké info.  
Problém bol s tým, že ako sa to pohodlne a jednoducho prepojí? Zo stránky užívateľ odošle prvotný email a ďalej komunikácia prebieha mimo stránky cez emaily. Znamená to, že potvrdenie objednávky nie je isté čo ak sa ďalej v komunikácii zistí nejaký problém a nakoniec o objednávku nebude záujem... Nemôžeme s istotou hneď po odoslaní prvotnej správy pridať užívateľovi položku do objednávok. Ako pohodlné riešenie pre obe strany by bolo vytvorenie vlastnej komunikácie priamo na stránke namiesto emailov. Ale to prišlo mne i ujovi už ako príliš, tak sme to zavrhli a povedali, že to za to nestojí. (Aj tak by to nebolo AŽ tak užitočné...)  
Ide o to že to nefunguje ako nejaký eshop, kde si dám niečo do košíka zaplatím a je isté že som si vec kúpil, tu sa pošle dopytovací email a obchod sa dohodne cez maily, takže stránka nevie či obchod prebehol úspešne, takže nevie či objednávku môže pridať do objednávok užívateľa.
- <u>Zrušilo sa upozorňovanie na servisnú prehliadku.</u> Kvôli zrušeniu objednávok teraz nemáme info o tom, ktorý stroj si užívateľ kúpil, takže ho nemôžme kontaktovať s tým, že je potrebná prehliadka stroja. (Pre aukčné ponuky by sa dalo, ale práve tam to nie je potrebné, požadované bolo iba pri nových)


- **Aukcia:**

**Akcie (queries) pre neprihláseného užívateľa v aukcii:**  
*Užívateľ má záujem o stroj:*
- Je zákazník v tabuľke? (kontroluje sa na vš. položky: meno, priezvisko...; stačilo by kontrolovať iba dočasných, ale to by bolo trochu čudné, lebo môhli by sme mať 2 rovnakých užívateľov, ktorí sú jeden dočasný a druhý nie):
  - Áno:
    - Je Dočasný?
      - Áno:
        - Vytvorí sa záznam v tabuľke UžívateľAukčnáPonuka (Uchádzač) ak tam ešte nebol, ináč sa len upraví*.
      - Nie: 
        - Chyba- Užívateľ existuje a má učet. Prosím, prihláste sa.
  - Nie:
    - Vytvorí sa záznam v tabuľke Zákazník, a tiež v UžívateľAukčnáPonuka (Uchádzači).  

**NOVÉ POŽIADAVKY (AUKCIA):**
- Minimálny vklad musí byť aspoň 100€ (napr.). Hodnota je fixná pre všetky aukčné ponuky. Aby nenavyšovali po centoch. -> Vyriešilo sa pridaním položky MinimálneNavýšenie do ~~AukcnaPonukaStrojaPage~~ AukčnáPonuka (klasická trieda).
- Do aukcie sa môže pridať aj typ stroju, ktorý neni medzi "novými". -> Vyriešilo sa pridaním Boolu do tabuľky Stroje (zatiaľ som ten stĺpec nazval JeNový).

*Čas aukčnej ponuky vypršal:*
1. Nájde sa riadok s najvyššou CenovouPonukou v tabuľke UžívateľAukčnáPonuka (Uchádzač)- to je výherca.
2. Máme výhercove ID (z predch. kroku), nájdeme ho v tabuľke Zákazník a pošleme mail.
3. Každý Zákazník, ktorý je dočasný a má len jediný výskyt v tabuľke UžívateľAukčnáPonuka (Uchádzač) musí byť z tabuľky Zákazník odstránený.
4. Všetky riadky s ID končiacej sa aukčnej ponuky sa musia vymazať z tabuľky UžívateľAukčnáPonuka (Uchádzač).

(*Takto sa vyhneme problému, kedy by sa niekto snažil zaplniť celú tabuľku s dočasnými užívateľmi, lebo nevytvára sa vždy nový záznam, ale kontroluje sa či už taký existuje a ak áno tak sa iba upravuje)

- **AukčnáPonuka:**  
  - Mohol som dať dátum a čas kedy má končiť aukčná ponuka do tabuľky, ale to by sa skomplikovalo "resetovanie" v prípade nezáujmu. Chcem aby sa o to systém staral sám. Ak ubehne čas a nikto sa neprihlási, chcem aby sa odpočet resetol, preto potrebujem mať info nie o tom kedy presne má skončiť (dátum a čas), ale o aký čas má skončiť (timespan). Takže ak dôjde na 0 a bude sa resetovať, znovu sa len nastaví na pôvodnú hodnotu. (V prípade dátumu by sa to muselo nejak komplikovane počítať. Toto mi príde ľahšie.)  
  A teraz... kde tento timespan uložím? Napadlo mi do databázy alebo spravím nejakú obalovú runtime triedu. Do databázy mi to nesedí, lebo to nie je niečo čo by som potreboval persistovať i potom ako ponuka skončí (áno mohol by som spraviť novú tabuľku, ktorá by "obalila" tú pôvodnu a v novej by bol timespan a tá by sa mazala ale to je zbytočné a pracné). Príde mi lepšie vytvoriť runtime triedu, ktorá obalí objekt Entity frameworku (aukčnú ponuku) a pridá k nej položku timespanu, jakmile ponuka nebude aktívna, runtime classa sa zahodí, ale zmeny ktoré sa na nej vykonali budú všetky persitované v tabuľke AukčnéPonuky (aukčnú ponuku chceme persistovať aby si užívateľ, admin vedeli (aktívne) aukčné ponuky zobrazovať).
  - Tiež potom padla myšlienka, že prečo máme tabuľku uchádzačov o ponuku, veď to je tiež len niečo, čo nás zaujíma len po dobu kým je ponuka aktívna, a potom "porazených" uchádzačov budeme mazať. Tak prečo uchádzačov o ponuku nepresunieme tiež do runtime classy? Povedal som si, že nevieme koľko uchádzačov bude, a preto bude asi lepší nápad ich ukladať do databázy ako držať ich všetkých v pamäti. Čo sa týka premazávania, tak to nie je niečo čo by užívateľ nejako vnímal- neni to tak že užívateľ stlačí tlačidlo a za niečím teraz musí čakať... deje sa to na pozadí.

- **Form:** 
  - Bude brať ako parameter, že odkiaľ sa píše ten form (či od strojov, či z kontaktov; do predmetu emailu sa môže dať táto informácia napr.). Tým pádom sa môže jedna komponenta formu využívať na viacerých miestach.
  - Bude mať 2 zobrazovacie módy: pre prihlásených a pre neprihlásených.

- **AlertYesNoComponent:** Bude brať text ako parameter.

- **GlobálnePremenné:** Bude obsahovať static položky, to budú nejaké všeobecné nastavenia pre všetkých rovnaké. Ale zároveň potrebujem nejaké instančné položky pre každého užívateľa iné (napr. si tam chcem uložiť inštanciu prihláseného užívateľa). <del>Preto som sa rozhodol ju naimplementovať ako sigleton, 1 inštancia na užívateľa.</del>

---

# Otázky:

- Titulnú fotku stroja dať osobitne do tabuľky Stroje alebo môže to fungovat tak, že v tabuľke FotkyStrojov prvá nájdená sa dá ako titulná? (Nemení sa poradie v tabuľke/databázi, nie??)
- Je v poriadku mať globálnu premennú, kde si uložím inštanciu užívateľa s odkazom na "databázove veci" (myslím objekty z Entity Frameworku)?
- Tabuľky UžívateliaAukčnéPonuky (Uchádzači) a AukčnéPonuky vyzerá že už nepotrebujem mať v databázi, mám to vyhodiť a budú iba ako runtime triedy? (Potrebujem ich len kým sú aktívne. Po predaní môžu zaniknúť.)