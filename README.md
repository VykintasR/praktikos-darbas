# praktikos_darbas
Reikalingos bibliotekos:
Beždžionės paleidimui per CLI:
•	.NET 7.0
•	RestSharp 110.2.0
•	Newtonsoft.Json 13.0.3
•	Microsoft.Extensions.Configuration.Json 7.0.0
•	Microsoft.Extensions.FileProviders.Abstractions
•	Microsoft.Extensions.FileProviders.Physical
•	Microsoft.Extensions.Primitives
•	Microsoft.Extensions.Configuration.FileExtensions
•	NUnit 3.13.3
•	CommandLineParser 2.9.1
•	Npgsql 7.0.4
Papildomai projekto unit testų paleidimui iš Visual Studio aplinkos:
•	NUnit3TestAdapter 4.4.2
•	NUnit.Analyzers 3.6.1
•	Microsoft.NET.Test.Sdk 17.5.0
•	Coverlet.collector 3.1.2

Instrukcija beždžionės paleidimui:
1) Susikonfigūruoti PostgreSQL duomenų bazę su rezultatų lentele,
Užklausa:
CREATE TABLE Tests
(
    id SERIAL PRIMARY KEY,
    category TEXT NOT NULL,
    success BOOLEAN NOT NULL,
    timeout INTEGER NOT NULL,
    test_time INTERvAL NOT NULL,
   deployment_http_status INTEGER NOT NULL,
    error_message TEXT NULL,
    request_data JSONB NOT NULL,
    result_data JSONB NOT NULL,
    created_at TIMESTAMP NOT NULL 
);
2) Užpildyti appsetings.json failą.
3) Paleisti per CLI su norimais parametrais.
CLI parametrai:
-–timeout – nurodomas maksimalus sveikas skaičius laiko limitui minutėmis
-–testcount – nurodomas atliekamų testų skaičius
-–random – užsakyti serverį parenkant visiškai atsitiktinius parametrus. Šis parametras nesuderinamas su kitais 4 žemiau išvardytais filtrais
-–region pavadinimas – užsakyti serverius tik nurodytame regione
-–plan pavadinimas – užsakyti konkretų produktą
-–image pavadinimas – užsakyti serverį su nurodytu OS šablonu
-–category pavadinimas – užsakyti serverį iš nurodytos kategorijos
 Šiuos 4 filtrus galima kombinuoti. Nenurodyti parametrai bus parenkami atsitiktinai iš galimų variantų pritaikius jau nurodytus filtrus ant visų planų sąrašo.