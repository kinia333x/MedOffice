# MedOffice
Aplikacja do zarządzania wizytami w gabinecie lekarskim

## 1. Dokumenty
- MANUAL (wszystkie informacje)<br>
https://docs.google.com/document/d/1y662gQPYb_3-R6Bz5RXJw_ghvIDmWGRx8x4K-bRzWdg/edit#
- Story<br>
https://docs.google.com/document/d/1uNUYRBC8jlA_VWNYqrqetDmsL_LsP-AVHQMjIRt9M7M/edit
- Produkt Backlog<br>
https://docs.google.com/document/d/1_Lq5d2hyQl01RMG5Yn_n8R8bdRPZnOx4fb5GYlJ5xw0/edit
- Dokument analizy wymagań
https://docs.google.com/document/d/1_jgzJ1696wAgdxjbADbQU4wZOmXTf-qWF2P2xqXf6tY/edit
- Grupa na facebooku - SI - (prywatna)
- Trello (prywatne)

## 2. Zasady code review:
- Każdy tworzy sobie imiennego brancha i commituje swoje zmiany na niego.
- Jeżeli ktoś robi na raz więcej tasków opierających się na różnych fragmentach kodu, tworzy sobie kolejne imienne branche na     osobne taski, np: imie, imie2, imie3 itd.
- Nikt nie wprowadza zmian bezpośrednio do mastera/PRODUCTU/cudzych branchy.
- Gdy ktoś uzna że jego kod działa, jest poprawnywny i nadaje się do wcielenia do projektu:<br>
  należy otworzyć pull request ze swojego brancha <b>imiennego</b> -> do brancha <b>master</b>.
- Wtedy reviewer pobiera brancha do siebie i sprawdza kod:<br>
  czy się kompiluje, czy działa i czy trzyma się konwencji.
- Jeżeli reviewer stwierdzi że kod jest w porządku robi merge: z brancha <b>imiennego</b> ->  do <b>mastera</b> i usuwa   brancha.<br>
  W przeciwnym wypadku robi close pull request, a autor poprawia kod.
- Jeżeli przy próbie mergowania branchy pojawi się konflikt niech rozwiąże go:<br>
  1.autor kodu, 2.reviewer, 3.ktokolwiek z grupy gdy pojawiają się jakiekolwiek wątpliwości.
- Po rozwiązaniu konfliktu należy sprawdzić czy aplikacja na pewno się kompiluje i czy działa jak powinna.
<br><br>
Co środę będziemy na zajęciach mergować mastera z branchem PRODUCT, gdzie ostatecznie będzie gotowy system. 

**Kto jest reviewerem dla kogo:**<br/>
- Kinga<->Wojtek
- Gosia<->Kornel
- Weronika<->Kamil<br/>
*(robiłam losowanie, jak ktoś nie wierzy, to możemy jeszcze raz razem zrobić :) )*

**Przyjęte ustalenia co do wyglądu kodu:** <br>
https://docs.google.com/document/d/1jI9NBwEGcKrm57fBraK6lldwR0N2Qc_Ih5w9qB_xgv0/edit

## 3. Definition of done
- Kod się kompiluje
- Kod jest poprawny pod względem konwencji
- Kod wykonuje dokładnie to co było zdefiniowane w tasku
- Program działa bez błędów i problemów, wprowadzone zmiany nie wpływają na jakość innych funkcjonalności
- Jest przetestowany (automatycznie lub recznie).
- Zatwierdzony przez reviewera.
- Dodane funkcjonalności są dokładnie opisane w DAWie!

## 4. Reszta informacji > MANUAL.
