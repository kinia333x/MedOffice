# MedOffice
Aplikacja do zarządzania wizytami w gabinecie lekarskim

## 1. Środowisko
- Visual Studio 2017<br/>
  Z doinstalowanymi pakietami: 
  - Programowanie aplikacji klasycznych dla platformy .NET
  - Opracowywanie zawartości dla platformy ASP.NET i sieci Web

- Git na windowsa
- GitHub Desktop

## 2. Dokumenty
- Story<br>
https://docs.google.com/document/d/1uNUYRBC8jlA_VWNYqrqetDmsL_LsP-AVHQMjIRt9M7M/edit
- Produkt Backlog<br>
https://docs.google.com/document/d/1_Lq5d2hyQl01RMG5Yn_n8R8bdRPZnOx4fb5GYlJ5xw0/edit
- Sprint Backlog<br>
https://docs.google.com/document/d/1OrL3krDmn4ZKPcQ2hZ8aEffAenilBgcOQ6aSVMeS94w/edit
- Dokument analizy wymagań
- Grupa na facebooku - SI - (prywatna)
- Trello (prywatne)

## 3. Zasady code review:
- Każdy commituje swoje zmiany na swojego imiennego brancha.
- Nikt nie wprowadza zmian bezpośrednio do mastera/PRODUCTU/cudzych branchy.
- Gdy ktoś uzna że jego kod działa, jest poprawnywny i nadaje się do wcielenia do projektu:<br>
  należy otworzyć pull request ze swojego brancha <b>imiennego</b> -> do brancha <b>master</b>.
- Wtedy reviewer pobiera brancha do siebie i sprawdza kod:<br>
  czy się kompiluje, czy działa i czy trzyma się konwencji.
- Jeżeli reviewer stwierdzi że kod jest w porządku robi merge: z brancha <b>imiennego</b> ->  do <b>mastera</b>.<br>
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

## 4. Definition of done
- Kod się kompiluje
- Kod jest poprawny pod względem konwencji
- Kod wykonuje dokładnie to co było zdefiniowane w tasku
- Program działa bez błędów i problemów, wprowadzone zmiany nie wpływają na jakość innych funkcjonalności
- Jest przetestowany (automatycznie lub recznie).
- Zatwierdzony przez reviewera.

## 5. Pisać jak najwiecej komentarzy do kodu! [BARDZO WAŻNE]

## 6. Tutoriale
- Asp.net
  - [streszczenie po polsku](http://kurs.aspnetmvc.pl/MVC/)
  - praktyka:
    - [podstawy](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/index)
    - [tworzenie użytkowników](https://docs.microsoft.com/en-us/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset)
    - [tworzenie roli użytkowników](https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97)

## 7. Podglądanie bazy danych w Visual Studio
`View` -> `Server Explorer` -> `Data Connection` -> `DefaultConnection` -> `Tables` <br/>

## 8. Git z konsoli (można to samo robic w GitHub Desktop):

- Pobieranie pierwszy raz:<br/>
`Git clone adres_url_repozytorium`

- Po dodaniu zmian:<br/>
`Git status` - sprawdzamy co trzeba dodac do śledzenia<br/>
`Git add odpowiedznie_foldery_pliki` - dodajemy co potrzeba<br/>
`Git commit -m "komentarz"`<br/>
`Git checkout twoj_branch` - zamiana gałęzi z master na twoją<br/>
`Git push` - wysyła zmiany<br/>

- Jesli nie poszło, prawdopodobnie ktoś dodał zmiany od momentu twojego pobrania. Trzeba: (to jeszcze uszczegółowimy, narazie nie wiemy)<br/>
`Git pull`<br/>
`Git push`

## 9. Rozwiązania różnych problemów
- Kiedy przy `git add` wyskakuje błąd "permission denied"- zamknij Visuala i spróbuj jeszcze raz.
- Z wszystkimi innymi problemami pisać natychmiast na grupę albo do kogoś. Nawet jeśli są banalne i głupie- ktoś może już przez to przebrnął i będzie szybciej niż szukanie po Stackach Overflowach. Byle nie stać w miejscu.
