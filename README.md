# Media Downloader M

A Windows desktop application for browsing available media formats and downloading video or audio from YouTube and SoundCloud links. The interface is available in English, German, and Arabic.

## Features

- Fetches the formats reported for a URL and displays each format's ID, media type, extension, resolution, frame rate, codecs, estimated size, bitrate, and notes.
- Filters the format list by video-and-audio, video-only, or audio-only formats.
- Downloads one selected format, or combines a selected video-only format and audio-only format. A video-only selection can also be paired with the best available audio.
- Offers quick video modes for best quality, up to 720p, and up to 1080p, plus audio modes for MP3 or the source format.
- Converts selected audio to MP3 and merges separate video/audio streams with FFmpeg.
- Supports playlist downloads, an optional `cookies.txt` file, a selectable output folder, progress, and cancellation.
- Downloads and updates `yt-dlp` automatically. FFmpeg is downloaded when needed or on request.

## Requirements

- Windows (the application targets `net8.0-windows`; the bundled FFmpeg download is for 64-bit Windows).
- .NET 8 SDK to build from source. Running a framework-dependent build requires the .NET 8 Desktop Runtime.
- An internet connection to retrieve `yt-dlp`, FFmpeg when required, and media.

The project has no third-party NuGet package references. On first launch, `yt-dlp.exe` is placed in `%LOCALAPPDATA%\MediaDownloader\tools`. FFmpeg and its required binaries are downloaded there when merging or MP3 conversion is requested; the download is approximately 80 MB. These tools are obtained from their upstream release/build sources.

## Build and run

```powershell
dotnet run --project .\MediaDownloaderM.csproj
```

Or build the solution in Visual Studio 2022 with the .NET 8 SDK and Windows Forms support installed:

```powershell
dotnet build .\MediaDownloaderM.slnx
```

## Use

1. Start the application and paste a YouTube or SoundCloud URL.
2. Select **Fetch formats** to inspect formats currently reported by `yt-dlp`, or choose a quick mode.
3. Optionally select a format in the table. To combine separate streams, select exactly one video-only row and one audio-only row. For a single video-only row, enable the option to merge the best available audio.
4. Choose the save folder and, if needed, enable playlist downloading or provide a `cookies.txt` file.
5. Start the download. Approve the FFmpeg download if prompted for merging or MP3 conversion.

SoundCloud links switch the empty-table quick mode to MP3 when one of the video quick modes is selected. When a format is selected manually, the chosen format and conversion options determine the result.

## Important notes

- Available formats and extensions depend on what the source currently provides and what `yt-dlp` can access. The application does not promise a fixed set of formats; examples may include MP4, WebM, M4A, or other source-provided formats. MP3 output requires FFmpeg.
- Site changes, network conditions, regional availability, and access restrictions can affect format listing and downloads. An optional cookies file may help with content the user is otherwise authorized to access. Treat cookies as credentials: keep the file private and never commit or share it.
- If FFmpeg is unavailable, the application can continue with the original selected format, but cannot merge separate streams or convert to MP3.
- The repository contains source code, not a prebuilt release package. No installer or automatic application updater is included.
- Media extraction is provided through the third-party `yt-dlp` project. This application is not affiliated with or endorsed by YouTube, SoundCloud, `yt-dlp`, or FFmpeg.

## Responsible use

**This project is intended for education, learning, knowledge sharing, supporting open-source software, and enriching the software ecosystem. The author and contributors do not authorize or encourage copyright infringement or unauthorized downloads and disclaim responsibility for users' misuse; each user is responsible for ensuring they have the necessary rights and follow platform terms and applicable laws.**

Download only content that you own or are explicitly authorized to download. Follow YouTube's and SoundCloud's terms, copyright rules, and all applicable laws. This project does not claim to bypass DRM, paywalls, or access restrictions.

---

# Media Downloader M (Deutsch)

Eine Windows-Desktopanwendung, mit der verfügbare Medienformate angezeigt und Videos oder Audiodateien von YouTube- und SoundCloud-Links heruntergeladen werden können. Die Benutzeroberfläche ist auf Englisch, Deutsch und Arabisch verfügbar.

## Funktionen

- Ruft die für eine URL gemeldeten Formate ab und zeigt ID, Medientyp, Dateiendung, Auflösung, Bildrate, Codecs, geschätzte Größe, Bitrate und Hinweise an.
- Filtert die Liste nach Video mit Audio, nur Video oder nur Audio.
- Lädt ein ausgewähltes Format herunter oder kombiniert ein ausgewähltes Videoformat ohne Audio mit einem Audioformat. Ein einzelnes Videoformat ohne Audio kann mit dem bestverfügbaren Audio ergänzt werden.
- Bietet Schnellmodi für Video in bester Qualität, bis 720p und bis 1080p sowie Audio als MP3 oder im Quellformat.
- Konvertiert ausgewähltes Audio in MP3 und führt getrennte Video-/Audiostreams mit FFmpeg zusammen.
- Unterstützt Playlist-Downloads, eine optionale `cookies.txt`-Datei, einen wählbaren Ausgabeordner, Fortschrittsanzeige und Abbruch.
- Lädt `yt-dlp` automatisch herunter und aktualisiert es. FFmpeg wird bei Bedarf oder auf Anforderung heruntergeladen.

## Voraussetzungen

- Windows (Zielplattform ist `net8.0-windows`; der FFmpeg-Download ist für 64-Bit-Windows).
- .NET 8 SDK zum Erstellen aus dem Quellcode. Zum Ausführen eines Framework-abhängigen Builds wird die .NET 8 Desktop Runtime benötigt.
- Internetverbindung zum Abrufen von `yt-dlp`, bei Bedarf FFmpeg und den Medien.

Das Projekt enthält keine Drittanbieter-NuGet-Paketverweise. Beim ersten Start wird `yt-dlp.exe` unter `%LOCALAPPDATA%\MediaDownloader\tools` abgelegt. FFmpeg und benötigte Binärdateien werden bei Bedarf für das Zusammenführen oder die MP3-Konvertierung heruntergeladen; der Download umfasst ungefähr 80 MB. Die Programme stammen aus den jeweiligen Upstream-Release- bzw. Build-Quellen.

## Erstellen und starten

```powershell
dotnet run --project .\MediaDownloaderM.csproj
```

Alternativ kann die Lösung in Visual Studio 2022 mit installiertem .NET 8 SDK und Windows-Forms-Unterstützung erstellt werden:

```powershell
dotnet build .\MediaDownloaderM.slnx
```

## Verwendung

1. Anwendung starten und einen YouTube- oder SoundCloud-Link einfügen.
2. **Formate abrufen** auswählen, um die aktuell von `yt-dlp` gemeldeten Formate anzuzeigen, oder einen Schnellmodus verwenden.
3. Optional ein Format in der Tabelle auswählen. Zum Zusammenführen genau eine Zeile „nur Video“ und eine Zeile „nur Audio“ markieren. Bei einer einzelnen Videozeile ohne Audio die Option zum Ergänzen des bestverfügbaren Audios aktivieren.
4. Speicherordner wählen und bei Bedarf Playlist-Download aktivieren oder eine `cookies.txt`-Datei angeben.
5. Download starten. Bei Bedarf den FFmpeg-Download für das Zusammenführen oder die MP3-Konvertierung bestätigen.

Bei SoundCloud-Links wechselt der Schnellmodus bei leerer Formattabelle von einem Video-Schnellmodus automatisch zu MP3. Bei manueller Formatauswahl bestimmen das gewählte Format und die Konvertierungsoptionen das Ergebnis.

## Hinweise

- Verfügbare Formate und Dateiendungen hängen davon ab, was die Quelle aktuell bereitstellt und worauf `yt-dlp` zugreifen kann. Es gibt keine Garantie für eine feste Formatauswahl; möglich sind beispielsweise MP4, WebM, M4A oder andere von der Quelle bereitgestellte Formate. Für MP3 wird FFmpeg benötigt.
- Änderungen an Websites, Netzwerkbedingungen, regionale Verfügbarkeit und Zugriffsbeschränkungen können die Anzeige und den Download beeinflussen. Eine optionale Cookie-Datei kann bei Inhalten helfen, auf die der Benutzer ohnehin zugriffsberechtigt ist. Cookies sind Zugangsdaten: Datei privat halten und niemals einchecken oder weitergeben.
- Ist FFmpeg nicht verfügbar, kann das ausgewählte Originalformat weiterhin heruntergeladen werden; getrennte Streams können dann nicht zusammengeführt und Audio nicht in MP3 konvertiert werden.
- Das Repository enthält Quellcode, kein vorgefertigtes Release-Paket. Ein Installationsprogramm und ein automatischer Anwendungs-Updater sind nicht enthalten.
- Die Medienextraktion erfolgt über das Drittanbieterprojekt `yt-dlp`. Diese Anwendung ist nicht mit YouTube, SoundCloud, `yt-dlp` oder FFmpeg verbunden und wird von ihnen nicht unterstützt.

## Verantwortungsvolle Nutzung

**Dieses Projekt dient der Bildung, dem Lernen, dem Wissensaustausch, der Unterstützung von Open-Source-Software und der Bereicherung des Software-Ökosystems. Der Autor und die Mitwirkenden genehmigen oder fördern weder Urheberrechtsverletzungen noch unbefugte Downloads und lehnen die Verantwortung für einen Missbrauch durch Nutzer ab; jeder Nutzer ist selbst dafür verantwortlich, die erforderlichen Rechte zu besitzen und die Plattformbedingungen sowie geltende Gesetze einzuhalten.**

Laden Sie nur Inhalte herunter, die Ihnen gehören oder für deren Download Sie ausdrücklich berechtigt sind. Beachten Sie die Nutzungsbedingungen von YouTube und SoundCloud, das Urheberrecht und alle geltenden Gesetze. Dieses Projekt beansprucht nicht, DRM, Bezahlschranken oder Zugriffsbeschränkungen zu umgehen.

---

# Media Downloader M (العربية)

تطبيق سطح مكتب لنظام Windows يعرض صيغ الوسائط المتاحة ويحمّل الفيديو أو الصوت من روابط YouTube وSoundCloud. واجهة البرنامج متاحة بالإنجليزية والألمانية والعربية.

## المزايا

- يجلب الصيغ التي يتيحها الرابط ويعرض معرّف الصيغة ونوع الوسائط والامتداد والدقة ومعدل الإطارات والترميزات والحجم التقريبي ومعدل البت والملاحظات.
- يتيح تصفية القائمة لعرض الفيديو مع الصوت، أو الفيديو فقط، أو الصوت فقط.
- يحمّل صيغة واحدة محددة، أو يدمج صيغة فيديو بلا صوت مع صيغة صوت. ويمكن إضافة أفضل صوت متاح إلى صيغة فيديو واحدة بلا صوت.
- يوفر أوضاعاً سريعة للفيديو بأفضل جودة وحتى 720p أو 1080p، وللصوت بصيغة MP3 أو بالصيغة الأصلية.
- يحوّل الصوت المحدد إلى MP3 ويدمج مسارات الفيديو والصوت المنفصلة باستخدام FFmpeg.
- يدعم تنزيل قوائم التشغيل، وملف `cookies.txt` اختياري، واختيار مجلد الحفظ، وعرض التقدم، والإلغاء.
- ينزّل `yt-dlp` ويحدّثه تلقائياً. يُنزّل FFmpeg عند الحاجة أو عند طلب تثبيته.

## المتطلبات

- Windows (يستهدف المشروع `net8.0-windows`، ونسخة FFmpeg التي ينزّلها مخصصة لويندوز 64 بت).
- حزمة تطوير .NET 8 للبناء من المصدر. يتطلب تشغيل بناء يعتمد على إطار العمل تثبيت .NET 8 Desktop Runtime.
- اتصال بالإنترنت لتنزيل `yt-dlp` وFFmpeg عند الحاجة والوسائط.

لا يحتوي المشروع على مراجع لحزم NuGet خارجية. عند التشغيل لأول مرة، يُحفظ `yt-dlp.exe` في `%LOCALAPPDATA%\MediaDownloader\tools`. وعند الحاجة إلى دمج المسارات أو تحويل الصوت إلى MP3، يُنزّل FFmpeg والملفات الثنائية المطلوبة؛ ويبلغ حجم التنزيل نحو 80 ميغابايت. تُنزّل الأدوات من مصادر الإصدارات والبناء الأصلية الخاصة بها.

## البناء والتشغيل

```powershell
dotnet run --project .\MediaDownloaderM.csproj
```

أو ابنِ الحل باستخدام Visual Studio 2022 بعد تثبيت .NET 8 SDK ودعم Windows Forms:

```powershell
dotnet build .\MediaDownloaderM.slnx
```

## طريقة الاستخدام

1. شغّل البرنامج والصق رابط YouTube أو SoundCloud.
2. اختر **جلب الصيغ** لاستعراض الصيغ التي يتيحها `yt-dlp` حالياً، أو اختر أحد الأوضاع السريعة.
3. يمكنك تحديد صيغة من الجدول. للدمج، حدّد صفاً واحداً للفيديو فقط وصفاً واحداً للصوت فقط. وإذا حددت صيغة فيديو واحدة بلا صوت، فعّل خيار دمج أفضل صوت متاح.
4. اختر مجلد الحفظ، وفعّل تنزيل قائمة التشغيل أو حدّد ملف `cookies.txt` عند الحاجة.
5. ابدأ التنزيل. وافق على تنزيل FFmpeg عند طلبه للدمج أو تحويل الصوت إلى MP3.

عند إدخال رابط SoundCloud وترك جدول الصيغ دون تحديد، يحوّل البرنامج الوضع السريع من الفيديو إلى MP3. أما عند تحديد صيغة يدوياً، فتتحدد النتيجة بحسب الصيغة وخيارات التحويل.

## ملاحظات مهمة

- تعتمد الصيغ والامتدادات المتاحة على ما يوفره المصدر حالياً وما يستطيع `yt-dlp` الوصول إليه. لا يضمن البرنامج مجموعة ثابتة من الصيغ؛ فقد تظهر مثلاً MP4 أو WebM أو M4A أو صيغ أخرى يوفرها المصدر. يتطلب إخراج MP3 وجود FFmpeg.
- قد تؤثر تغييرات المواقع والاتصال بالإنترنت والتوفر حسب المنطقة وقيود الوصول على جلب الصيغ والتنزيل. قد يساعد ملف الكوكيز الاختياري للوصول إلى محتوى يحق للمستخدم الوصول إليه أصلاً. ملفات الكوكيز بيانات اعتماد حساسة؛ احتفظ بها خاصة ولا ترفعها أو تشاركها.
- إذا لم يتوفر FFmpeg، يمكن تنزيل الصيغة الأصلية المحددة، لكن لا يمكن دمج المسارات المنفصلة أو تحويل الصوت إلى MP3.
- يحتوي المستودع على الشيفرة المصدرية، وليس حزمة إصدار جاهزة. لا يتضمن مثبتاً أو محدثاً تلقائياً للتطبيق.
- يعتمد استخراج الوسائط على مشروع `yt-dlp` الخارجي. هذا التطبيق غير تابع لـ YouTube أو SoundCloud أو `yt-dlp` أو FFmpeg ولا يحظى بتأييدها.

## الاستخدام المسؤول

**يهدف هذا المشروع إلى التعليم والتعلّم والمعرفة وتبادلها ودعم البرمجيات مفتوحة المصدر وإثراء المحتوى البرمجي. لا يجيز المؤلف والمساهمون انتهاك حقوق الملكية أو التنزيل غير المصرّح به ولا يشجعونهما، ويخلون مسؤوليتهم عن إساءة استخدام البرنامج؛ ويتحمّل كل مستخدم مسؤولية التأكد من امتلاكه الحقوق اللازمة والتزامه بشروط المنصات والقوانين المعمول بها.**

نزّل فقط المحتوى الذي تملكه أو لديك تصريح صريح بتنزيله. التزم بشروط YouTube وSoundCloud وحقوق النشر والقوانين المعمول بها. لا يدّعي هذا المشروع تجاوز DRM أو أنظمة الدفع أو قيود الوصول.
