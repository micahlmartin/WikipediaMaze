/// <reference path="jquery-1.3.2-vsdoc.js" />
/// <reference path="activebar2.js" />
/// <reference path="jquery-ui-1.7.2.custom.min.js" />


var minRepToVote = 100;

$(document).ready(function () {

    // Datepicker
    $('.datepicker').datepicker({
        inline: true
    });

    //hover states on the static widgets
    $('#dialog_link, ul#icons li').hover(
					function () { $(this).addClass('ui-state-hover'); },
					function () { $(this).removeClass('ui-state-hover'); }
				);

    //Makes the logo navigate back to home page. 
    $("#logo").click(function () {
        location.href = "/";
    });

    //Notification
    $.ajax({
        type: "GET",
        url: "/account/getnotifications",
        dataType: "json",
        cache: false,
        success: function (data) {
            var notifyBar = $('#user-panel');
            for (i = 0; i < data.length; i++) {
                var result = data[i];
                notifyBar.append("<p id='messageId-" + result.Id.toString() + "' class='notify-user'>" + result.Message + " <a class='user-panel-switch' title='Close message' href='#'>X</a></p>");
            }

            if(data.length > 0)
                notifyBar.slideDown();

            $("#user-panel a").click(function () {
                var parent = $(this).parent("p");
                parent.fadeOut();
                var id = parent.attr("id");
                var messageId = id.substring(10);

                $.ajax({
                    type: "POST",
                    url: "/account/ClearNotification/" + messageId,
                    dataType: "json",
                    cache: false
                })
            });
        }
    });

    //Voting
    var puzzleIds = new Array();
    var puzzleIdCounter = 0;

    $(".votes-arrows").each(function () {
        puzzleIds[puzzleIdCounter] = $(this).children("input").attr("value");
        puzzleIdCounter++;
    });

    //Solutions in User Profile
    $("div.user-display-solution").css("cursor", "pointer")
                .mouseover(function () { $(this).css("background", "#f5f5ff") })
                .mouseout(function () { $(this).css("background", "none") })
                .click(function () { location.href = "/puzzles/" + $(this).children("input").val().toString() });


    $("span[class^='vote-up']").bind("click", function (e) {
        var temp = $(this);
        var puzzleId = $(this).siblings("input").attr("value");
        $.post("/puzzles/VoteOnPuzzle?", { puzzleid: puzzleId, voteType: 1 }, function (data, status) {
            if (data.ErrorMessage == null && status == "success") {
                $(temp).parent(".votes-arrows").siblings(".votes").children("span").html(data.VoteCount);
                switch (data.VoteType) {
                    case 1:
                        $(temp).attr("class", "vote-up-selected");
                        $(temp).siblings("span[class^='vote-down']").attr("class", "vote-down");
                        break;
                    case 0:
                        $(temp).attr("class", "vote-up");
                        $(temp).siblings("span[class^='vote-down']").attr("class", "vote-down");
                        break;
                    case -1:
                        $(temp).attr("class", "vote-up");
                        $(temp).siblings("span[class^='vote-down']").attr("class", "vote-down-selected");
                        break;
                }
            }
            else {
                if ($(temp).siblings(".vote-notification").length == 0) {
                    $(temp).siblings(".vote-down").after("<div class='vote-notification' style='display:block;'><h2>" + data.ErrorMessage + "</h2> (click on this box to dismiss)</div>");
                    $(temp).siblings(".vote-notification").click(function (e) { $(this).remove() });
                }
            }
        }, "json");
    });

    $("span[class^='vote-down']").bind("click", function (e) {
        var temp = $(this);
        var puzzleId = $(this).siblings("input").attr("value");
        $.post("/puzzles/VoteOnPuzzle?", { puzzleId: puzzleId, voteType: -1 }, function (data, status) {
            if (data.ErrorMessage == null && status == "success") {
                $(temp).parent(".votes-arrows").siblings(".votes").children("span").html(data.VoteCount);
                switch (data.VoteType) {
                    case 1:
                        $(temp).siblings("span[class^='vote-up']").attr("class", "vote-up-selected");
                        $(temp).attr("class", "vote-down");
                        break;
                    case 0:
                        $(temp).siblings("span[class^='vote-up']").attr("class", "vote-up");
                        $(temp).attr("class", "vote-down");
                        break;
                    case -1:
                        $(temp).siblings("span[class^='vote-up']").attr("class", "vote-up");
                        $(temp).attr("class", "vote-down-selected");
                        break;
                }
            }
            else {
                if ($(temp).siblings(".vote-notification").length == 0) {
                    $(temp).after("<div class='vote-notification' style='display:block;'><h2>" + data.ErrorMessage + "</h2> (click on this box to dismiss)</div>");
                    $(temp).siblings(".vote-notification").click(function (e) { $(this).remove() });
                }
            }
        }, "json");
    });
});

var puzzles = {

    init: function () {
        $(document).ready(function () {

            if (typeof puzzleIds !== 'undefined') {
                $.post("/puzzles/votes", { puzzleIds: puzzleIds }, function (data) {
                    $(data).each(function (i, e) {
                        var vote = eval(e);

                        if (vote.voteType > 0) {
                            $("#puzzle-" + vote.puzzleId + " .votes-arrows .vote-up").removeClass("vote-up").addClass("vote-up-selected");
                        } else if(vote.voteType < 0) {
                            $("#puzzle-" + vote.puzzleId + " .votes-arrows .vote-down").removeClass("vote-up").addClass("vote-down-selected");
                        }

                    });
                });
            }

        });
    }
}


