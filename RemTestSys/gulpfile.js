/// <binding AfterBuild='debug' Clean='clean' />
var gulp = require("gulp");
var del = require("del");
var uglify = require("gulp-uglify");
var concat = require("gulp-concat");

var paths = {
    scripts: ["Scripts/**/*.js"],
};

gulp.task("clean_webroot", function () {
    return del(["wwwroot/scripts/**/*"]);
});

gulp.task("clean_source", function () {
    return del(["Scripts/**/*.js"]);
});

gulp.task(
    "production", function (done) {
        gulp.src(paths.scripts)
            .pipe(concat("testing.js"))
            .pipe(uglify())
            .pipe(gulp.dest("wwwroot/scripts"));
        done();
    }
);

gulp.task(
    "debug", function (done) {
        gulp.src(paths.scripts)
            .pipe(concat("testing.js"))
            .pipe(gulp.dest("wwwroot/scripts"));
        done();
    }
);