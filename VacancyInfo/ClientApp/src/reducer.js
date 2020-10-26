import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'

const initialState = {
    vacancyName: 'React',
    isFirstSearch: true,
    selectedAreas: [],
    avarageSalary: '',
    averageRegionSalary: '',
    isLoading: false
};

export const getAllAreas = createAsyncThunk(
    'VacancyInfo/Areas',
    async (_, { getState, dispatch }) => {
        const response = await fetch('api/Vacancy/GetAreasJson')
            .then(res => {
                return res.json();
            })
            .catch(err => console.error('Areas', err))
        return response;
    }
)
export const getAvarageSalary = createAsyncThunk(
    'VacancyInfo/AvarageSalary',
    async (_, { getState, dispatch }) => {
        const response = await fetch('api/Vacancy/GetAvarageSalary')
            .then(res => {
                return res.json();
            })
            .catch(err => console.error('AvarageSalary', err))
        return response;
    }
)
export const getVacancies = createAsyncThunk(
    'VacancyInfo/getVacancies',
    async (_, { getState, dispatch }) => {
        const { isFirstSearch, vacancyName } = getState();
        dispatch(cleanInfo());
        await fetch(`api/Vacancy/Vacancies?name=${vacancyName}`)
            .then(res => {
                if (isFirstSearch) {
                    dispatch(setFirstSearch(false));
                    dispatch(getAllAreas());
                }
                dispatch(getAvarageSalary());
            })
            .catch(err => console.error(err))
    }
)
export const getAverageRegionSalary = createAsyncThunk(
    'VacancyInfo/getAverageRegionSalary',
    async (payload, { getState, dispatch }) => {
        const { id } = payload;
        let response = '';
        if (id) {
            response = await fetch(`api/Vacancy/GetAverageRegionSalary?areaId=${id}`)
                .then(res => {
                    return res.json();
                })
                .catch(err => console.error(err));
        }
        return response;
    }
)

const slice = createSlice({
    name: 'VacancyInfo',
    initialState,
    reducers: {
        setVacancyName(state, action) {
            state.vacancyName = action.payload;
        },
        setFirstSearch(state, action) {
            state.isFirstSearch = action.payload;
        },
        selectAreas(state, action) {
            state.selectedAreas = action.payload;
        },
        cleanInfo(state, actoin) {
            state.selectedAreas = [];
            state.averageRegionSalary = '';
            state.avarageSalary = '';
        }
    },
    extraReducers: {
        [getVacancies.fulfilled]: (state, action) => {
            state.isLoading = false;
            console.log(action.payload);
        },
        [getVacancies.pending]: (state, action) => {
            state.isLoading = true;
        },
        [getAllAreas.fulfilled]: (state, action) => {
            console.log(action.payload);
            state.areas = action.payload;
        },
        [getAvarageSalary.fulfilled]: (state, action) => {
            console.log('avarageSalary', action.payload);
            state.avarageSalary = action.payload;
        },
        [getAverageRegionSalary.fulfilled]: (state, action) => {
            console.log('averageRegionSalary', action.payload);
            if (action.payload) {
                state.averageRegionSalary = action.payload;
            }
        }
    }
});

export default slice.reducer;

export const {
    setVacancyName,
    setFirstSearch,
    selectAreas,
    cleanInfo
} = slice.actions