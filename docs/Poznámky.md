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

- ~~Static class **PrihláseníUžívatelia** bude obsahovať zoznam všetkých užívateľov, takže keď sa niektorí z užívateľov bude snažiť prihlásiť, skontroluje sa či už prihlásený je, ak áno, nájde sa v tomto zozname jeho inštancia a vráti sa mu jeho model, takto sa nebudeme musiet vždy dotazovať priamo z databázy a zároveň môže byť užívateľ prihlásený na viacerých PC. Ak by sa v zozname nenašiel, tak sa naťahajú dáta z databázy a vytvorí sa nová inštancia modelu.~~ DBContext nie je thread-safe, musí sa dať ináč...

**!!! Veľký UPDATE: !!!** 

- ~~<u>Zrušili sa Objednávky</u> (kúpa stroja, prídavných zariadení, zemné práce, oprava stroja) ako entinty. Existovala myšlienka, že užívateľ si v profile bude môcť objednávky vylistovať a mal by o nich nejaké info.  
Problém bol s tým, že ako sa to pohodlne a jednoducho prepojí? Zo stránky užívateľ odošle prvotný email a ďalej komunikácia prebieha mimo stránky cez emaily. Znamená to, že potvrdenie objednávky nie je isté čo ak sa ďalej v komunikácii zistí nejaký problém a nakoniec o objednávku nebude záujem... Nemôžeme s istotou hneď po odoslaní prvotnej správy pridať užívateľovi položku do objednávok. Ako pohodlné riešenie pre obe strany by bolo vytvorenie vlastnej komunikácie priamo na stránke namiesto emailov. Ale to prišlo mne i ujovi už ako príliš, tak sme to zavrhli a povedali, že to za to nestojí. (Aj tak by to nebolo AŽ tak užitočné...)  
Ide o to že to nefunguje ako nejaký eshop, kde si dám niečo do košíka zaplatím a je isté že som si vec kúpil, tu sa pošle dopytovací email a obchod sa dohodne cez maily, takže stránka nevie či obchod prebehol úspešne, takže nevie či objednávku môže pridať do objednávok užívateľa.~~
- ~~<u>Zrušilo sa upozorňovanie na servisnú prehliadku.</u> Kvôli zrušeniu objednávok teraz nemáme info o tom, ktorý stroj si užívateľ kúpil, takže ho nemôžme kontaktovať s tým, že je potrebná prehliadka stroja. (Pre aukčné ponuky by sa dalo, ale práve tam to nie je potrebné, požadované bolo iba pri nových)~~  
**EDIT**: Nakoniec vyzerá, že pridám "chat", takže formy nebudú posielať správy na mail, ale konverzácie budú prebiehať v aplikácií. Tým pádom <u>nebudem musieť rušiť</u> tieto funkcionality.


- **Aukcia:**

**Akcie (queries) pre neprihláseného užívateľa v aukcii:**  
*Užívateľ má záujem o stroj:*

- Je zákazník v tabuľke? (~~kontroluje sa na vš. položky: meno, priezvisko...; stačilo by kontrolovať iba dočasných, ale to by bolo trochu čudné, lebo môhli by sme mať 2 rovnakých užívateľov, ktorí sú jeden dočasný a druhý nie~~ EDIT: prečo by to bolo čudné? Aj tak sa po skončení budú mazať alebo nie? Myslím, že by som sa mohol ku každému neprihlásenému užívateľovi správať ako k novému... PK v tabuľke je predsa ID, nie stĺpce ako meno, email atď. Zrejme bude treba toto trochu upraviť až budem implementovať.):
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
- Ak máme záujemcov:
  1. Nájde sa riadok s najvyššou CenovouPonukou v tabuľke UžívateľAukčnáPonuka (Uchádzač)- to je výherca.
  2. Máme výhercove ID (z predch. kroku), nájdeme ho v tabuľke Zákazník a pošleme mail.
  3. Každý Zákazník, ktorý je dočasný a má len jediný výskyt v tabuľke UžívateľAukčnáPonuka (Uchádzač) musí byť z tabuľky Zákazník odstránený.
  4. Všetky riadky s ID končiacej sa aukčnej ponuky sa musia vymazať z tabuľky UžívateľAukčnáPonuka (Uchádzač).  

- Ak nemáme záujemcov:
  1. Prvý krok je rovnaký, ale ak sa výherca nenájde, tak potom...
  2. Ku aktuálnemu času (čas skončenia ponuky) sa pripočíta timespan z triedy AukčnáPonuka a vytvorí sa tak nový dátum konca ponuky, a ten sa uloží do datbázy. Takto sa resetne ponuka.

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

- ~~Je v poriadku mať globálnu premennú, kde si uložím inštanciu užívateľa s odkazom na "databázove veci" (myslím objekty z Entity Frameworku)?~~ EDIT: DBContext nie je thread-safe, takže skôr by som asi spravil pre každé spojenie nový DBContext, ktorý by bol obalený ešte mojou triedou a volania komunikujúce s DB by boli "obalené" mutexami alebo niečim... JE aj lepšie riešenie?
  *Odpoveď:* Vraj sa zvykne pre každé spojenie s DB vytvárať nový DBContext, pretože aj keby sa medzitým keď ho máme vytvorený zmení stav v DB, tak to ten vytvorený objekt DBContextu nepozná... Overhead jeho vytvárania nie je taký veľký takže je v poriadku ho vytvárať častejšie.

---

## P3.

- **Chat:**  
Funkcionality chatu:
  - odoslať/prijať textové správy
  - len pri prihlásených užívateľoch... admin má možnosť potvrdiť obchod
  - pri neprihlásených užívateľoch... správa príde do adminovskej schránky, ALE kedže admin nemá komu odpísať (lebo užívateľ nemá účet, a teda ani kontaktné info), musí existovať nejaké políčko pre email/telefón, aby užívateľa vedel admin spätne kontaktovať

~~Stroje majú kategórie (pásové bagre, šmykom riadené...), každá kategória má vlastný typ tabuľky (info o strojoch). Rozhodol som sa do databázy dať iba <code>List\<string\></code>, ale v runtimu komponenta pri vytváraní ponuky nového stroja sa pozrie na kategóriu stroja a podľa toho poskytne form na vyplnenie tabuľky (podľa kategórie vieš, aký druh tabuľky potrebuješ).~~ EDIT: Prišlo mi to čudné, čo ak sa nejako zmenia properties v kóde (napr. poradie) a už by to nematchovalo na tabuľku alebo niečo... Radšej asi spravím ďalšie tabuľky pre šmykom riadené a pásové bagre, a tie budú dediť od tabuľky Stroje, a tam si môžem dať stĺpce podľa toho, čo potrebujem. Toto mi príde ako lepší nápad.

Všimol som si, že názvy kariet (ponuky) na detvaservise (stránka, ktorú používam ako predlohu) nie sú konzistentné... Niekedy názov pozostáva z kategórie + značka + model (Pásový bager Eurocomach 58 ZT), inokedy kategória + značka + nejaký popis (Vŕtacie zariadenie Auger Torque pre mini rýpadlá od 750 kg do 3 t), alebo dokonca "kategória" (preto "" lebo kategória je *šmykom riadené nakladače*, ale v názve je iba *nakladače*) + značka + model (Nakladač LOCUST 753). Preto som sa rozhodol zaviesť do tabuliek pre stroje a prídavné zariadena osobitný stĺpec pre názov. Pôvodne som myslel že názov vyskladám jednoducho iba z kategórie, značky a modelu. Ale myslím si, že by bol dobrý nápad defaultne adminovi predvyplniť názov s kategória + značka + model, a ak by si to chcel zmeniť, pridať tam niečo (napr. "pre mini rýpadlá od 750 kg do 3 t"), tak môže.

Prišiel mi ako dobrý nápad zaviesť **maďarskú notáciu** pri modeloch, napr. MaximumPower**Kw**.

Aby sa mi dobre pracovalo so záznamami v tabuľkách, vytvoril som si interface <code>IItem</code>, ktorý obsahuje id. Všetky tabuľky majú 1 id- teda až na tabuľu s uchádzačmi aukcie. Aby bolo všetko jednotné, tak som sa rozhodol zrušiť zložený PK (Užívateľ.PK a AukčnéPonuky.ID) a pridať nové id. Takže aj táto tabuľka bude mať 1 id, a teda bude pasovať do interfacu.

Pri vytváraní modelov v kóde som premýšľal, či dať súčiastky stroja ako list alebo ako kolekciu. Kolekcia by možno dávala väčší zmysel, pretože logicky stroj je ako taká množina súčiastok :-D Ale vybral som si list, pretože chcem mať pri iterovaní garantované poradie :)

Pýtal som sa a predávať sa môžu (hoc i na aukcii) stroje iba z kategórií: šmykom riadené nakladače, pásové bagre, pásové nakladače. => A preto som sa rozhodol zaviesť do modelu v programe enum s týmito kategóriami. Takisto som sa rozhodol pridať strojom property pre kategóriu (možno to neskôr pomôže výkonnosti; takisto queries budú vyzerať asi rozumenejšie keď budem mať rovnaký enum v tabuľkách pre stroje a prídavné zariadenia, bez enumu v tabuľke strojov by som musel zistiť čo to je za kategóriu podľa tabuľiek zderivovaných, a potom to kombinovať s enumom v prídavných zariadeniach... ak bude enum v oboch tabuľkách, tak to bude asi jasnejšie).

While doing data annotations I checked an appropriate max length for name. It is said to be 35 (or 70 for fullname) according to: https://stackoverflow.com/questions/30485/what-is-a-reasonable-length-limit-on-person-name-fields  
Also for email address: https://stackoverflow.com/questions/1199190/what-is-the-optimal-length-for-an-email-address-in-a-database  
Also, for phone numbers I chose 17 even though internet said its 15. Thats because format like 00421 123 456 789 might be accepted as well and this format has length 17.

Why to use **DbContextFactory:** https://docs.microsoft.com/en-us/ef/core/dbcontext-configuration/#using-a-dbcontext-factory-eg-for-blazor

Rozhodol som sa odstrániť enum ExcavatorCategory, pretože som si uvedomil, že by to mohlo užívateľa (admina) obmedzovať. Keď to bude string, je väčšia voľnosť v tom, čo sa tam môže napísať. A teda ponuka produktov môže byť flexibilnejšia (ak chce pridať niečo nové, nemusí čakať za mnou, aby som pridal položku do enumu). + Už už som šiel prídávať enum i pre značky strojov a príd. zariadení, ale po hovore s majiteľom, ktorý to chcel mať "voľnejšie", som si uvedomil, že enum by to zbytočne zostriktnil.

Premýšľam ako navrhnúť API k databáze (queries), konkrétne vylistovanie strojov a prídavných zariadení podľa značiek. Najprv som rozmýšľal, že by som mohol spraviť metódu, ktorá berie parameter string (značka), potom som si povedal, že enum by bol efektívnejší, ale to som zavrhol, pretože by sa stratila flexibilita (enumy sú hardkodnuté, admin by si nemohol pridať novú značku ak by chcel). Takisto som premýšľal, či by sa tu nejako nedal využiť GroupBy alebo aby som mal pre kažú značku osobitnú metódu... Nakoniec som sa rozhodol pre ten string, pretože mi to prišlo ako najflexibilnejšie riešenie.

Metódy na testovanie toho, či existuje zákazník/admin, som najprv chcel navrhnúť tak, že budú vraciať bool (myslel som si že sa využije sql ASK query, takže to bude efektívnejšie). Ale teraz premýšľam nad tým, že lepšie asi bude vraciať rovno inštanciu zákazníka/admina (príp. null). Myslím si, že ak sa budem pýtať na to, či zákazník/admin existuje, tak s ním rovno budem chcieť i pracovať (alebo aspoň vo väčšine prípadov, ak nie vo všetkých...). Dobre, tak teda nakoniec som sa rozhodol mať metódu, ktorá berie id, a takisto metódu, ktorá berie username a heslo. Obe budú vraciať inštanciu zákazníka/admina, akurát tá druhá bude vraciať nullable zákazníka/admina. Pretože ak už som raz mal inštanciu vybratú z DB a pracoval som s ňou, a len si ju chcem znova naťahať z DB (napr. id predám ako parameter do komponenty, ktorá má niečo vykresliť napr. profil užívateľa pre admina), tak je efektívnejšie hľadať len za pomoci id. Na druhú stranu, keď sa užívateľ prihlasuje, tak id nepoznám. Preto potrebujem aj metódu, ktorá vie vrátiť inštanciu zákazníka/admina podľa username a password (null ak údaje nesprávne, a teda nenájde záznam- tu je to očakávané, že nenájde).  
 
~~Pri vytváraní API, som chcel mať metódy~~
```C#
public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsByAsync(string? excavatorCategory = null);
public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(string? category = null);
public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(string? brand = null);
```
~~pre jednoduché používianie, ale kedže signatúra je rovnaká. Musel som zmeniť na:~~
```C#
public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsByExcavatorCategoryAsync(string excavatorCategory);
public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsByCategoryAsync(string category);
public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsByBrandAsync(string brand);
```
Edit: Tak nakoniec budem mať len metódu `public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync();` a až keď budeme potrebovaŤ filtrovať sa to vyfiltruje... Pretože potom som sa zamyslel a moŽností podľa čoho filtrovať je viac (kategória, značka...) a vytvárať pre každú novú metódu... ale počkať! Dobre tak nakoniec som sa rozhodol takto:
```C#
public Task<List<AdditionalEquipment>> GetAdditionalEquipmentsAsync(int numberOfAdditionalEquipments, int startIndex, string excavatorCategory, string? category = null, string? brand = null);
```
A podobne i pre stroje:
```C#
public Task<List<Excavator>> GetExcavatorsAsync(int numberOfExcavators, int startIndex, string? category = null, string ? brand = null, string? model = null);
```
(prvé parametre `int numberOfAdditionalEquipments, int startIndex` sú pre virtualizáciu)

Mal som problém s tým, že od .Net6 (myslím) Startup.cs nie je, a teda ani `Configure()` metóda, do ktorej sa ako parameter mal injectnúť `IDbContextFactory<BServisDbContext>` a spraviť `factory.CreateDbContext().Database.Migrate()`. Nakoniec sa to vyriešilo v Program.cs takto:
```C#
builder.Services.Configure<IDbContextFactory<BServisDbContext>>(factory =>
	factory.CreateDbContext().Database.Migrate()
);
```
**Prečo som použil staršie verzie nuget packagov aj keď existujú novšie?**  
Pretože s najnovšími mi nešlo robiť migrácie...

Konečne som našiel ako vyriešiť problém s top level statementami, teda že chýba Startup.cs s metódami ```ConfigureServices()``` a ```ConfigureServicesConfigue()```... https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60?view=aspnetcore-6.0&tabs=visual-studio#new-hosting-model (ctrl + f a "ConfigureServices is replaced with")

Zmenil som nastavenie v projektoch, aby sa Warningy brali ako Errory.

```GetExcavatorsAsync()``` má jeden z parametrov ```string```- kategóriu stroja. POZOR! Je v slovenčine kvôli tomu, že som sa rozprával s majiteľom a proste hodí sa aby to bol string a nie enum, aby mohli prípadne pridávať aj inej kategórie ľahko, aby mali voľnosť v tomto.

---

**Premenovanie** -> BServis (WebApp) na ServIS (WebApp). Nebude sa používať názov firmy, ale aplikácia má už vlastný názov.

Vymazanie **SparePartDetail** -> Mal som stránku pre detail náhradného dielu, dalo sa tam dostať cez tabuľku dielov, ktorá je v profilu (stačilo kliknúť na detail). Ale aj napriek tomu, že som to mal všetko naimplementované som sa rozhodol to zmazať, pretože všetko info bude vidno i na stránke s tabuľkou (dokonca to bude editovateľné!). Navyše, ak by som chcel ten detail spraviť poriadne, tak by som musel ešte nejako poriešiť to, aby keď klikneme na detail, a potom sa vrátime spať, bol stav formu (prípadne i tabu) rovnaký... Čiže musel by som robiť niečo naviac a mali by sme z toho rovnaký úžitok ako bez toho (dokonca menší lebo tam by nebola možnosť editovania)... Takže som to radšej zmazal.

**Virtializácia a zmena počtu** -> Na vylistovanie náhrandých dielov používam komponentu `Virtualize`. Keď kliknem na tlačidlo a zmažem náhradný diel, tak chcem aby riadok s daným náhradnym dielom zmizol hneď, a aby užívateľ nemusel refreshnuť stránku. Z nejakého dôvodu si ale Virtualize držal dáta a až keď užívateľ vyscrolloval mimo tabuľku a zase späť tak sa dáta aktualizovali... Problém sa vyriešil pomocou `RefreshDataAsync`. Viac tu: https://github.com/dotnet/aspnetcore/issues/26110 a tu https://stackoverflow.com/questions/65449283/blazor-virtualize-update-when-request-change

**Vytvoril som si vlastny Hook** -> `AfterValidSubmit` v `SparePartForm` (vyuzivam/nastavujem ho v `SparePartManagement`). Kebyze sa form vyuziva sam bez listeru tak tiez to bude fungovat, ak si tam chce pridat nejake ine spravani po validnom submitnuti, tak moze. Týmto spôsobom môžem využiť form a lister spolu ale zároveň vedia fungovať i každý zvlášť.

**zmena db providera** -> Z *Oracle* na *Pomelo*, pretože som nemohol robiť migrácie... ak som mal staršiu verziu packagov, mohol som robiť migrácie, ale nejaké z queries nefungovali dobre (problém s .Include()). Ak som updatol na najnovšiu verziu, queries fungovali dobre, ale nešli migrácie. Viackrát som sa na internete stretol s tým, že Pomelo je lepší provider(rýchlejšie reagujú + na nuget.org viac stiahnutí) a vraj ak ho zmením všetko bude fungovať. A áno po zmene všetko funguje a mám najnovšie verzie. (Ďalšie info tu: https://stackoverflow.com/questions/70224907/unable-to-resolve-service-for-type-microsoft-entityframeworkcore-diagnostics-idi)

**prečo klasický input checkbox namiesto InputCheckbox?** -> Pretože mi nejako zle fungoval... myslím, že konkrétne checked nechcel fungovať.
