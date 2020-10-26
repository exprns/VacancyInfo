import React, { useCallback, useEffect } from 'react';
import { TextField, Button } from '@material-ui/core'
import Autocomplete from '@material-ui/lab/Autocomplete'
import cn from 'classnames'
import { useSelector, useDispatch } from 'react-redux'
import {
    setVacancyName,
    getVacancies as getVacanciesAction,
    selectAreas,
    getAverageRegionSalary
} from '../../reducer';
import { Map } from '../../components/Map';
import { SideMenu } from '../../components/SideMenu';
import './Home.css'

export const Home = () => {
    const name = useSelector((state) => state.vacancyName);
    const isFirstSearch = useSelector((state) => state.isFirstSearch);
    const areas = useSelector((state) => state.areas);
    const avarageSalary = useSelector((state) => state.avarageSalary);
    const selectedAreas = useSelector((state) => state.selectedAreas);
    const averageRegionSalary = useSelector((state) => state.averageRegionSalary);
    const dispatch = useDispatch()
    const handleText = useCallback(
        (e) => dispatch(setVacancyName(e.target.value)),
        [dispatch]);

    const getVacancies = useCallback(() => {
        dispatch(getVacanciesAction())
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [dispatch])

    const handleChangeAreas = (event, newValue) => {
        dispatch(selectAreas(newValue));
    }

    useEffect(() => {
        dispatch(getAverageRegionSalary(selectedAreas));
    }, [dispatch, selectedAreas])

    const getOptionLabel = (option) => option.name;

    return (
        <div className={ "home" } >
            <div className={ cn("searchRow", { "searchRowIsSearch": !isFirstSearch }) } >
                <TextField
                    value={ name }
                    className={ cn("textField", { "textFieldWidthAnimation": !isFirstSearch }) }
                    placeholder="Работа моей мечты"
                    onChange={ handleText }
                />
                <Button
                    color="primary"
                    onClick={ getVacancies }
                >
                    Найти
            </Button>
            </div>
            {!isFirstSearch
                && <>
                    <div className={ "row" }>Средняя зарплата по России { avarageSalary ? `: ${avarageSalary}` : 'не найдена' }</div>
                    <div className={ cn("mapAndMenu", "row", { "visibleMapAndMenu": !isFirstSearch }) }>
                        { areas && areas.length && <Autocomplete
                            className={ "autocomplite" }
                            /* multiple */
                            id="areas"
                            onChange={ handleChangeAreas }
                            value={ selectedAreas }
                            options={ areas }
                            getOptionLabel={ getOptionLabel }
                            renderInput={ (params) => (
                                <TextField
                                    { ...params }
                                    variant="standard"
                                    placeholder="Регионы"
                                />
                            ) }
                        />
                        }
                    </div>
                    <div className={ "row" }>Средняя зарплата по выбранному региону { averageRegionSalary ? `: ${averageRegionSalary}` : 'не найдена' }</div>
                </>
            }

        </div >
    );
}
