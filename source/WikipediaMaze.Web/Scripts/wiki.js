/// <reference path="jquery-1.3.2-vsdoc.js" />

function ClickHandler(url) 
{
    var url = $(this).attr("href");
    if (url.indexOf('#') == 0) { return true; }
    if (ValidateUrl(url)) {
        var topic = url.split("/")[2];
        document.location = "/wiki?topic=" + topic;
    }
    return false;
}
function ValidateUrl(url) {
    var topicRegex = new RegExp("wiki/.+");

    return topicRegex.test(url);
}

var headerContent = "<div id='wikiheader'>\n";
headerContent += "\t<h1>Wikipedia Maze</h1>\n";
headerContent += "\t<p class='endtopic'>Looking for <span></span></p>\n";
headerContent += "\t<p class='step'>Step <span></span></p>\n";
headerContent += "\t<p class='level'>Average Steps <span></span></p>\n";
headerContent += "\t<p class='startover'></p>\n";
headerContent += "\t<p class='goback'></p>\n";
headerContent += "</div>";

$(document).ready(function() {
    $("body").prepend(headerContent);
    $("#wikiheader").click(function() { location.href = "/" });

    $.ajax({
        type: "GET",
        url: "/game/getpuzzleinfo",
        dataType: "json",
        cache: false,
        success: function(data) {
            if (data.IsBrowsing) {
                $("#wikiheader").html("<h1>Wikipedia Maze</h1> <p class='endtopic'>Browsing Topics. <a href='/' title='Wikipedia Maze'>Click here</a> if you would like to play the game.</p>");
            }
            else {
                $("#wikiheader p.endtopic span").html("<a title='Opens the topic you are looking for in a separate tab or window' href='" + data.EndTopicUrl + "' target='_blank'>" + data.EndTopic + "</a>");
                $("#wikiheader p.step span").html(data.StepCount.toString());
                $("#wikiheader p.level span").html(data.PuzzleLevel.toString());
                $("#wikiheader p.startover").html("<a href='/game/start/" + data.PuzzleId.toString() + "'>Start Over</a>");
                if (data.StepCount > 1) {
                    $("#wikiheader p.goback").html("<a title='Go back to the previous topic' href='/game/goback/'>Go Back</a>");
                }
            }
        }
    });


    $("a").click(function() {
        var url = $(this).attr("href");

        if (url.indexOf("wikimedia.org/") >= 0 || url.indexOf("wiktionary.org/") >= 0 || url.indexOf("wikisource.org/") >= 0 || url.indexOf("wikiversity.org/") >= 0 || url.indexOf("mediawiki.org/") >= 0 || url.indexOf("wikinews.org/") >= 0 || url.indexOf("wikiquote.org/") >= 0 || url.indexOf("wikibooks.org/") >= 0 || url.indexOf("wikimediafoundation.org/") >= 0)
            return false;

        if (url.indexOf('#') == 0) { return true; }
        if (ValidateUrl(url)) {
            var topic = url.split("/")[2];
            document.location = "/wiki?topic=" + topic;
        }
        return false;
    });

});