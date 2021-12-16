const { src } = require('gulp');

var gulp = require('gulp');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var sass = require('gulp-sass');
var cleanCSS = require('gulp-clean-css');
var concatCss = require('gulp-concat-css');

const paths = require('../paths.json');
const sassOptions = require('../sassOptions.js');

gulp.task('govuk-js', () => {
    return src([
        'node_modules/govuk-frontend/govuk/*.js',
        'node_modules/govuk-frontend/govuk/vendor/**.js',
        'node_modules/govuk-frontend/govuk/components/**/*.js',
    ])
        .pipe(gulp.dest('wwwroot/govuk/javascripts'));
});

gulp.task('copy-js', function () {
    return src([
        'node_modules/jquery/dist/jquery.min.js',
        'Frontend/src/javascripts/cookie-banner.js'
    ])
        .pipe(concat('all.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-provider-search-js', function () {
    return src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.js',
        'Frontend/src/javascripts/provider-search.js'
    ])
        .pipe(concat('provider-search.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-registration-upload-js', function () {
    return src([
        'Frontend/src/javascripts/registration-upload.js'
    ])
        .pipe(concat('registration-upload.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-assessment-upload-js', function () {
    return src([
        'Frontend/src/javascripts/assessment-upload.js'
    ])
        .pipe(concat('assessment-upload.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-result-upload-js', function () {
    return src([
        'Frontend/src/javascripts/result-upload.js'
    ])
        .pipe(concat('result-upload.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-english-maths-question-js', function () {
    return src([
        'Frontend/src/javascripts/english-maths-question.js'
    ])
        .pipe(concat('english-maths-question.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-user-guide-js', function () {
    return src([
        'Frontend/src/javascripts/user-guide.js'
    ])
        .pipe(concat('user-guide.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-cookies-js', function () {
    return src([
        'Frontend/src/javascripts/cookies.js'
    ])
        .pipe(concat('cookies.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-registration-specialism-question-js', function () {
    return src([
        'Frontend/src/javascripts/registration-specialism-question.js'
    ])
        .pipe(concat('registration-specialism-question.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-session-timeout-js', function () {
    return src([
        'Frontend/src/javascripts/session-timeout.js'
    ])
        .pipe(concat('session-timeout.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-data-export-js', function () {
    return src([
        'Frontend/src/javascripts/data-export.js'
    ])
        .pipe(concat('data-export.js'))
        .pipe(minify({
            noSource: true,
            ext: {
                min: '.min.js'
            }
        }))
        .pipe(gulp.dest(paths.dist.defaultJs));
});

gulp.task('copy-assets', () => {
    return src(paths.src.defaultAssets)
        .pipe(gulp.dest(paths.dist.defaultAssets));
});

gulp.task('sass', () => {
    return src(paths.src.default)
        .pipe(sass(sassOptions))
        .pipe(gulp.dest(paths.mid.default));
});

gulp.task('merge-css', gulp.series('sass', function () {
    return src([
        'node_modules/accessible-autocomplete/dist/accessible-autocomplete.min.css',
        "Frontend/src/stylesheets/css/*.css"
    ])
        .pipe(concatCss("main.css"))
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(gulp.dest(paths.dist.default));
}
));

