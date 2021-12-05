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

