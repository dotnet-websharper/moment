// Bundle entry point for the sample.
//
// WebSharper generates the application module (`build/<name>.js`), which imports
// the npm `moment-timezone` package. The moment locales are opt-in side-effect
// imports, so register the ones the sample offers here before loading the app
// (they all share the single `moment` instance).
import 'moment/locale/en-gb'
import 'moment/locale/fr'
import 'moment/locale/de'
import 'moment/locale/es'
import 'moment/locale/ja'
import 'moment/locale/ru'
import 'moment/locale/ar'
import 'moment/locale/zh-cn'

import './build/WebSharper.Moment.Sample.js'
