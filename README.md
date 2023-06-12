# Opis

Backend systemu wspomagającego pracę komisu sprzedającego tanie pojazdy. Uwzględniono następujące funkcjonalności: 
 * logowanie oraz rejestrację nowego użytkownika
 * przywrócenie hasła
 * potwierdzenie/ usunięcie konta
 * możliwość zarządzania własnym kontem przez użytkownika
 * dodawanie, edycję i usuwanie ofert pojazdów
 * filtrowanie ofert
 * możliwość otrzymania maila z kodem QR prowadzącym do wybranych ofert
 * pojawiające się na stronie powiadomienia o nowych ofertach

Zrealizowano także testy ( jednostkowe oraz integracyjne ) w celu sprawdzenia poprawności działania powyższych funkcjonalności. Są to testy:
 * kontrolerów
 * repozytoriów
 * validatorów

## Użyte technologie

* .NET 7
* Entity Framework 7
* ASP.NET Core Identity
* POSTGRESQL
* FluentValidation
* AutoMapper
* xUnit


## Instalacja
Włączając backend możemy skorzystać z Dockera na przykład Docker Desktop, wówczas konieczne jest wykonanie komendy:

```bash
$ docker compose up
```
Chcąc zobaczyć działanie aplikacji możemy także wejść na stronę: [aplikacja](https://tanie-graty.netlify.app/) 