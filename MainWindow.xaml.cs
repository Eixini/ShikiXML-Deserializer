using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.Xml.Linq;
using Microsoft.Win32;

namespace ShikiXML;

public struct ShikiXMLStruct {
    public string AnimeTitle;
    public string AnimeType;
    public int NumberViewed;
    public int NumberFact;
}
public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }



    private void DeserializationButton_Click(object sender, RoutedEventArgs e) {

        OpenFileDialog SelectShikiXMLFile = new OpenFileDialog();
        SelectShikiXMLFile.Filter = "XML files (*.xml)|*.xml";

        Nullable<bool> openFileDialog = SelectShikiXMLFile.ShowDialog();

        if (openFileDialog == true) {

            List<ShikiXMLStruct> AnimeCompletedList = new List<ShikiXMLStruct>();
            List<ShikiXMLStruct> AnimeWatchingList = new List<ShikiXMLStruct>();
            List<ShikiXMLStruct> AnimePlanWatchList = new List<ShikiXMLStruct>();
            List<ShikiXMLStruct> AnimeDroppedList = new List<ShikiXMLStruct>();
            List<ShikiXMLStruct> AnimeRewatchingList = new List<ShikiXMLStruct>();
            List<ShikiXMLStruct> AnimeOnHoldList = new List<ShikiXMLStruct>();

            var shikiXMLFileName = SelectShikiXMLFile.FileName;

            // Загружаем документ для парсинга
            XDocument AnimeListXMLFile = XDocument.Load(shikiXMLFileName);
            XElement? RootAnimeListElement = AnimeListXMLFile.Element("myanimelist");

            if(RootAnimeListElement != null) {
                foreach (var anime in RootAnimeListElement.Elements()) {

                    ShikiXMLStruct currentAnime = new ShikiXMLStruct();
                    string animeUserStatus = "";

                    // Получение необходимых данных для пользовательского списка аниме
                    foreach (var animeElement in anime.Elements()) {
                        if (animeElement.Name == "series_title")
                            currentAnime.AnimeTitle = animeElement.Value;
                        if (animeElement.Name == "series_type")
                            currentAnime.AnimeType = animeElement.Value;
                        if (animeElement.Name == "my_watched_episodes")
                            currentAnime.NumberViewed = Convert.ToInt32(animeElement.Value);
                        if (animeElement.Name == "series_episodes")
                            currentAnime.NumberFact = Convert.ToInt32(animeElement.Value);
                        if (animeElement.Name == "my_status")
                            animeUserStatus = animeElement.Value;
                    }

                    // Проверка статуса аниме и добавление в соответствующую коллекцию
                    switch(animeUserStatus) {
                        case "Completed":
                            AnimeCompletedList.Add(currentAnime);
                            break;
                        case "Watching":
                            AnimeWatchingList.Add(currentAnime);
                            break;
                        case "Plan to Watch":
                            AnimePlanWatchList.Add(currentAnime);
                            break;
                        case "Dropped":
                            AnimeDroppedList.Add(currentAnime);
                            break;
                        case "Rewatching":
                            AnimeRewatchingList.Add(currentAnime);
                            break;
                        case "On-Hold":
                            AnimeOnHoldList.Add(currentAnime);
                            break;
                    }

                } // End of iteration anime list

            }

            // Запись результата в текстовый файл
            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                + Path.DirectorySeparatorChar + "myanimelist.txt";

            StreamWriter writer = new StreamWriter(FilePath, true);
            if(AnimeCompletedList.Count != 0) {
                int count = 1;
                writer.WriteLine("----------    Просмотрено     ----------");
                foreach (ShikiXMLStruct anime in AnimeCompletedList) {
                    writer.Write(count + ". ");
                    writer.Write("\"" + anime.AnimeTitle + "\"" + " ");
                    writer.Write(anime.AnimeType + " ");
                    writer.Write("[" + anime.NumberViewed + "/" + anime.NumberFact + "]");
                    writer.Write('\n');
                    count++;
                }
            }
            if (AnimeWatchingList.Count != 0) {
                int count = 1;
                writer.WriteLine("----------    В процессе      ----------");
                foreach (ShikiXMLStruct anime in AnimeWatchingList) {
                    writer.Write(count + ". ");
                    writer.Write("\"" + anime.AnimeTitle + "\"" + " ");
                    writer.Write(anime.AnimeType + " ");
                    writer.Write("[" + anime.NumberViewed + "/" + anime.NumberFact + "]");
                    writer.Write('\n');
                    count++;
                }
            }
            if (AnimePlanWatchList.Count != 0) {
                int count = 1;
                writer.WriteLine("----------    Запланировано       ----------");
                foreach (ShikiXMLStruct anime in AnimePlanWatchList) {
                    writer.Write(count + ". ");
                    writer.Write("\"" + anime.AnimeTitle + "\"" + " ");
                    writer.Write(anime.AnimeType + " ");
                    writer.Write("[" + anime.NumberViewed + "/" + anime.NumberFact + "]");
                    writer.Write('\n');
                    count++;
                }
            }
            if (AnimeDroppedList.Count != 0) {
                int count = 1;
                writer.WriteLine("----------    Брошено     ----------");
                foreach (ShikiXMLStruct anime in AnimeDroppedList) {
                    writer.Write(count + ". ");
                    writer.Write("\"" + anime.AnimeTitle + "\"" + " ");
                    writer.Write(anime.AnimeType + " ");
                    writer.Write("[" + anime.NumberViewed + "/" + anime.NumberFact + "]");
                    writer.Write('\n');
                    count++;
                }
            }
            if (AnimeRewatchingList.Count != 0) {
                int count = 1;
                writer.WriteLine("----------    Пересмотр       ----------");
                foreach (ShikiXMLStruct anime in AnimeRewatchingList) {
                    writer.Write(count + ". ");
                    writer.Write("\"" + anime.AnimeTitle + "\"" + " ");
                    writer.Write(anime.AnimeType + " ");
                    writer.Write("[" + anime.NumberViewed + "/" + anime.NumberFact + "]");
                    writer.Write('\n');
                    count++;
                }
            }
            if (AnimeOnHoldList.Count != 0) {
                int count = 1;
                writer.WriteLine("----------    Отложено        ----------");
                foreach (ShikiXMLStruct anime in AnimeOnHoldList) {
                    writer.Write(count + ". ");
                    writer.Write("\"" + anime.AnimeTitle + "\"" + " ");
                    writer.Write(anime.AnimeType + " ");
                    writer.Write("[" + anime.NumberViewed + "/" + anime.NumberFact + "]");
                    writer.Write('\n');
                    count++;
                }
            }

            writer.Close();

        } // End of "if(openFileDialog == true)" code
    } // End of DeserializationButton_Click code
} // End of MainWindow code

