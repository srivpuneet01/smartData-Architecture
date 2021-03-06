﻿$(function () {

    $(document).on('click', '#scheduler .k-scheduler-views a.k-link', function () {
        $("#scheduler").data("kendoScheduler").dataSource.read()
    });
    $(document).on('click', '#scheduler .k-scheduler-navigation a.k-link', function () {
        $("#scheduler").data("kendoScheduler").dataSource.read()
    });
    $.ajax({
        cache: false,
        async: true,
        type: "GET",
        url: $URL._GetSchedulerData,
        data: {},
        success: function (data) {

            function onSelect(e) {
                var dataItem = this.dataSource.view()[e.item.index()];
            };
            function onClose() {
                var oldlist = groupusers(data);
                var schedulerControl = $("#scheduler").data("kendoScheduler");
                schedulerControl.resources[0].dataSource.data(oldlist);
                schedulerControl.view(schedulerControl.view().name);
            };
            function onChange() {
                var oldlist = groupusers(data);
                var schedulerControl = $("#scheduler").data("kendoScheduler");
                schedulerControl.resources[0].dataSource.data(oldlist);
                schedulerControl.view(schedulerControl.view().name);
            };
            $("#users").kendoMultiSelect({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: data.users,
                select: onSelect,
                close: onClose,
                change: onChange,
                maxSelectedItems: 5
            });
            var multiSelect = $("#users").data("kendoMultiSelect");
            //multiSelect.value([multiSelect.dataSource.options.data[0].value, multiSelect.dataSource.options.data[1].value, multiSelect.dataSource.options.data[2].value]);


            function scheduler_dataBinding(e) {
                console.log("dataBinding");
            }

            function scheduler_dataBound(e) {
                console.log("dataBound");
            }

            function scheduler_save(e) {

                console.log("save");
            }

            function scheduler_remove(e) {
                console.log("remove");
            }

            function scheduler_cancel(e) {
                console.log("cancel");
            }

            function scheduler_change(e) {
                var start = e.start; //selection start date
                var end = e.end; //selection end date
                var slots = e.slots; //list of selected slots
                var events = e.events; //list of selected Scheduler events

                var message = "change:: selection from {0:g} till {1:g}";

                if (events.length) {
                    message += ". The selected event is '" + events[events.length - 1].title + "'";
                }

                console.log(kendo.format(message, start, end));
            }

            function scheduler_edit(e) {
                console.log("edit");
            }

            function scheduler_add(e) {

                console.log("add");
            }

            function scheduler_moveStart(e) {
                console.log("moveStart");
            }
            function scheduler_appointment(e) {
                alert("Appointment code goes here")
            }
            function scheduler_move(e) {
                console.log("move");
            }

            function scheduler_moveEnd(e) {
                console.log("moveEnd");
            }

            function scheduler_resizeStart(e) {
                console.log("resizeStart");
            }

            function scheduler_resize(e) {
                console.log("resize");
            }

            function scheduler_resizeEnd(e) {
                console.log("resizeEnd");
            }

            function scheduler_navigate(e) {
                console.log(kendo.format("navigate:: action:{0}; view:{1}; date:{2:d};", e.action, e.view, e.date));
            }
            //Added to stop multiple event at same time slot
            function occurrencesInRangeByResource(start, end, resourceFieldName, event, resources) {
                var scheduler = $("#scheduler").getKendoScheduler();

                var occurrences = scheduler.occurrencesInRange(start, end);

                var idx = occurrences.indexOf(event);
                if (idx > -1) {
                    occurrences.splice(idx, 1);
                }

                event = $.extend({}, event, resources);

                return filterByResource(occurrences, resourceFieldName, event[resourceFieldName]);
            }

            function filterByResource(occurrences, resourceFieldName, value) {
                var result = [];
                var occurrence;

                for (var idx = 0, length = occurrences.length; idx < length; idx++) {
                    occurrence = occurrences[idx];
                    if (occurrence[resourceFieldName] === value) {
                        result.push(occurrence);
                    }
                }
                return result;
            }
            function attendeeIsOccupied(start, end, event, resources) {
                var occurrences = occurrencesInRangeByResource(start, end, "attendee", event, resources);
                if (occurrences.length > 0) {
                    return true;
                }
                return false;
            }

            function roomIsOccupied(start, end, event, resources) {
                var occurrences = occurrencesInRangeByResource(start, end, "roomId", event, resources);
                if (occurrences.length > 0) {
                    return true;
                }
                return false;
            }
            function checkAvailability(start, end, event, resources) {

                if (attendeeIsOccupied(start, end, event, resources)) {
                    setTimeout(function () {
                        alert("This person is not available in this time period.");
                    }, 0);

                    return false;
                }

                if (roomIsOccupied(start, end, event, resources)) {
                    setTimeout(function () {
                        alert("This room is not available in this time period.");
                    }, 0);

                    return false;
                }

                return true;
            }

            $.contextMenu({
                selector: '#scheduler .k-event',
                callback: function (key, menuId, options, name) {
                    var scheduler = $("#scheduler").data("kendoScheduler");
                    var dataSource = scheduler.dataSource;
                    var uid = $(this).attr("data-uid");
                    if (key == "edit") {
                        var dataItem = dataSource.getByUid(uid);
                        scheduler.editEvent(dataItem);
                    } else if (name == "Status") {
                        // Write code here
                    } else if (name == "Physician") {
                        // Write code here
                    } else if (name == "Service") {
                        // Write code here
                    } else if (key == "history") {
                        // Write code here
                    }
                },
                items: {
                    "edit": {
                        name: "Edit",
                        icon: "edit"
                    },
                    "appointmentStatus": {
                        name: "Status",

                    },
                    "physician": {
                        name: "Physician",

                    },
                    "service": {
                        name: "Service",

                    },
                    "history": {
                        name: "Audit Log"
                    }
                }
            });

            $("#scheduler").kendoScheduler({

                height: 600,
                views: [
                    "day",
                    "workWeek",
                    { type: "week", selected: true },
                    "month",
                ],
                timezone: "Etc/UTC",
                dataBinding: scheduler_dataBinding,
                dataBound: scheduler_dataBound,
                save: scheduler_save,
                remove: scheduler_remove,
                edit: scheduler_edit,
                appointmentStatus: scheduler_appointment,
                add: scheduler_add,
                cancel: scheduler_cancel,
                change: scheduler_change,
                moveStart: scheduler_moveStart,
                move: scheduler_move,
                moveEnd: scheduler_moveEnd,
                resizeStart: scheduler_resizeStart,
                resize: scheduler_resize,
                resizeEnd: scheduler_resizeEnd,
                navigate: scheduler_navigate,

                dataSource: {
                    sync: function () {
                        this.read();
                    },

                    transport: {
                        read:
                            {
                                url: $URL._GetAppointments,
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                            },
                        update: {
                            url: $URL._EditAppointments,
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                        },
                        create: {
                            url: $URL._CreateAppointments,
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                        },
                        destroy: {
                            url: $URL._DeleteAppointments,
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                        },
                        parameterMap: function (options, operation) {

                            if (operation === "read") {
                                var scheduler = $("#scheduler").data("kendoScheduler");
                                var result = {
                                    start: scheduler.view().startDate(),
                                    end: scheduler.view().endDate(),
                                }
                                return kendo.stringify(result);
                            }
                            else {

                                var postModel = {
                                    taskId: options.TaskID,
                                    title: options.Title,
                                    start: options.Start,
                                    end: options.End,
                                    startTimezone: options.StartTimezone,
                                    endTimezone: options.EndTimezone,
                                    description: options.Description,
                                    recurrenceId: options.RecurrenceId,
                                    recurrenceRule: options.RecurrenceRule,
                                    recurrenceException: options.RecurrenceException,
                                    ownerId: options.OwnerID,
                                    isAllDay: options.IsAllDay,
                                    patientId: options.PatientId,
                                    tempRecurrenceID: options.TempRecurrenceID,
                                }
                                return kendo.stringify(postModel);
                            }
                        }
                    },
                    requestEnd: function (e) {

                        if (e.type == "read") {
                            return;
                        }

                        if (e.type == "update" || e.type == "create" || (e.sender != null && e.sender.data().length > 0)) {
                            setTimeout(function () {
                                $('.k-scheduler-cancel').click();
                                $('#scheduler').data('kendoScheduler').dataSource.read();
                                $('#scheduler').data('kendoScheduler').refresh();
                            }, 1000);

                        }

                    },
                    schema: {
                        model: {
                            id: "taskId",
                            fields: {
                                taskId: { from: "TaskID", type: "number" },
                                title: { from: "Title", defaultValue: "No title", validation: { required: true } },
                                start: { type: "date", from: "Start" },
                                end: { type: "date", from: "End" },
                                startTimezone: { from: "StartTimezone" },
                                endTimezone: { from: "EndTimezone" },
                                description: { from: "Description" },
                                recurrenceId: { from: "RecurrenceID" },
                                recurrenceRule: { from: "RecurrenceRule" },
                                recurrenceException: { from: "RecurrenceException" },
                                ownerId: { from: "OwnerID", validation: { required: true } },
                                patientId: { from: "PatientId", validation: { required: true } },
                                isAllDay: { type: "boolean", from: "IsAllDay" },
                                tempRecurrenceID: { from: "TempRecurrenceID", type: "number" }
                            }
                        }
                    },
                },
                group: {
                    resources: ["ownerId"]
                },
                resources: [
                    {
                        field: "ownerId",
                        title: "User",
                        dataSource: groupusers(data.users)
                    },
                     {
                         field: "patientId",
                         title: "Patient",
                         dataSource: data.patient
                     }
                ]
            });
        },
        error: function (request, error) {
            if (request.statusText == "CustomMessage") {
                $("#spanError").html(request.responseText)
            }
        },

    });

});

function groupusers(data) {
    var multiSelect = $("#users").data("kendoMultiSelect");
    var oldlist = [];
    if (multiSelect._dataItems.length > 0) {
        for (var i = 0; i < multiSelect._dataItems.length; i++) { //data.searchResults.list.length
            oldlist.push({
                text: multiSelect._dataItems[i].text,
                value: multiSelect._dataItems[i].value,
            });
        };
    }
    return oldlist;
}