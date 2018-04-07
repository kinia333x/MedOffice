# MedOffice
Aplikacja do zarządzania wizytami w gabinecie lekarskim

## 1. Środowisko
- Visual Studio 2017<br/>
  Z doinstalowanymi pakietami: 
  - Programowanie aplikacji klasycznych dla platformy .NET
  - Opracowywanie zawartości dla platformy ASP.NET i sieci Web

- Git na windowsa
- Github Desktop

## 2. Dokumenty (Kinga dołączy to)
- Story
- Produkt Backlog
- Sprint Backlog
- Dokument analizy wymagań
- Grupa na facebooku- SI

## 3. Zasady code review: (Kinga popraw)
- Każdy commituje na swojego brancha.
- Reviewer sprawdza kod, czy trzyma się konwencji, czy jest generalnie poprawny.
- Następnie pobiera tego brancha i sprawdza czy aplikacja działa.
- Jeśli ok, reviewer łączy branch z masterem.

**Kto jest reviewerem dla kogo:**<br/>
- Kinga<->Wojtek
- Gosia<->Kornel
- Weronika<->Kamil<br/>
*(robiłam losowanie, jak ktoś nie wierzy, to możemy jeszcze raz razem zrobić :) )*

## 4. Definition of done (Kinga popraw)
- Kod działa.
- Jest przetestowany (automatycznie lub recznie).
- Zatwierdzony przez reviewera.

## 5. Pisać jak najwiecej komentarzy do kodu!

## 6. Tutoriale
- Asp.net
  - [streszczenie po polsku](http://kurs.aspnetmvc.pl/MVC/)
  - praktyka:
    - [podstawy](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/index)
    - [tworzenie użytkowników](https://docs.microsoft.com/en-us/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset)
    - [tworzenie roli użytkowników](https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97)


## 7. Git z konsoli (można to samo robic w GitHub Desktop):

- Pobieranie pierwszy raz:<br/>
`Git clone adres_url_repozytorium`

- Po dodaniu zmian:<br/>
`Git status` - sprawdzamy co trzeba dodac do śledzenia<br/>
`Git add odpowiedznie_foldery_pliki` - dodajemy co potrzeba<br/>
`Git commit -m "komentarz"`<br/>
`Git checkout twoj_branch` - zamiana gałęzi z master na twoją<br/>
`Git push` - wysyła zmiany<br/>

- Jesli nie poszło, prawdopodobnie ktoś dodał zmiany od momentu twojego pobrania. Trzeba: (to jeszcze uszczegółowimy, narazie nie wiemy)<br/>
`Git pull`
`Git push`

## 8. Rozwiązania różnych problemów
- kiedy przy `git add` wyskakuje błąd "permission denied"- zamknij Visuala i spróbuj jeszcze raz
- z wszystkimi innymi problemami pisać natychmiast na grupę. Byle nie stać w miejscu
